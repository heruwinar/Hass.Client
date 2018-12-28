using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Hass.Client.HassApi.Websocket;
using Newtonsoft.Json.Linq;

namespace Hass.Client.HassApi
{
    public class WsAPI : IHassAPI
    {
        public static IHassAPI Instance { get; set; }

        public event EventHandler Authenticated;

        public event EventHandler<ResultReceivedEventArgs> ResultReceived;

        public event EventHandler<StateChangedEventArgs> StateChanged;

        private int currentId = 0;

        private MessageWebSocket ws;

        private Dictionary<int, RequestMessage> sentMessages = new Dictionary<int, RequestMessage>();

        private RequestMessage subscribeEventMessage = RequestMessage.CreateSubscribeStateChangeEventMessage();

        public WsAPI(Uri endpoint, string apiPassword)
        {
            Endpoint = endpoint;
            APIPassword = apiPassword;
        }

        public Uri Endpoint { get; private set; }

        public string APIPassword { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public int NextId()
        {
            lock(this)
            {
                return ++currentId;
            }
        }

        public async Task<bool> ConnectAsync()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            EventHandler onAuthenticated = null;

            onAuthenticated = (s, e) =>
            {
                Authenticated -= onAuthenticated;
                if (IsAuthenticated)
                {
                    tcs.SetResult(IsAuthenticated);
                }
                else
                {
                    tcs.TrySetException(new Exception("Authentication failed"));
                }
            };

            Authenticated += onAuthenticated;

            ws = new MessageWebSocket();
            ws.MessageReceived += OnWsMessageReceived;
            ws.Connect(Endpoint.ToString());

            return await tcs.Task;
        }

        private void OnWsMessageReceived(object sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            JObject json = JObject.Parse(args.Message);

            var response = ResponseMessage.Parse(this, json);

            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                ProcessResponseMessage(response, json);
            });

            LogAPIReceivedMessage(response.Type, json.ToString());
        }

        private void ProcessResponseMessage(ResponseMessage response, JObject json)
        {
            switch (response.Type)
            {
                case ResponseMessage.MessageType.Auth_required:
                    SendMessageAsync(RequestMessage.CreateAuthMessage(APIPassword));
                    break;
                case ResponseMessage.MessageType.Auth_ok:
                    SendMessageAsync(subscribeEventMessage);
                    OnAuthenticated(true, response);
                    break;
                case ResponseMessage.MessageType.Auth_invalid:
                    OnAuthenticated(false, response);
                    break;
                case ResponseMessage.MessageType.Event:
                    OnStateChanged(response.Event);
                    break;
                case ResponseMessage.MessageType.Result:
                    OnResultReceived(response);
                    break;
            }
        }

        private void LogAPIReceivedMessage(ResponseMessage.MessageType msgType, string message)
        {
            string fn = $"{DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss")}-{msgType}.json";

            string dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            System.IO.File.WriteAllText(System.IO.Path.Combine(dir, fn), message);
        }

        public async Task<ResponseMessage> SendMessageAsync(RequestMessage message)
        {
            int? msgId = message.Type == RequestMessage.MessageType.Auth
                ? null
                : new int?(NextId());

            string json = message.BuildRequestJson(msgId);

            sentMessages.Add(message.DetermineMessageId().GetValueOrDefault(), message);

            ws.Send(json);

            return await message.Task;
        }

        public RequestMessage FindRequestMessageOrDefault(int requestId)
        {
            if(requestId == subscribeEventMessage.DetermineMessageId().GetValueOrDefault())
            {
                return subscribeEventMessage;
            }
            RequestMessage reqMsg;
            sentMessages.TryGetValue(requestId, out reqMsg);
            return reqMsg;
        }

        public async Task<StateResult[]> ListStatesAsync()
        {
            ResponseMessage res = await SendMessageAsync(new RequestMessage(RequestMessage.MessageType.Get_states));
            return res.States.OrderBy(st => st.EntityId).ToArray();
        }

        private void OnAuthenticated(bool isAuthenticated, ResponseMessage authReponseMessage)
        {
            IsAuthenticated = isAuthenticated;

            OnResultReceived(authReponseMessage);

            Authenticated?.Invoke(this, EventArgs.Empty);
        }

        private void OnResultReceived(ResponseMessage message)
        {
            RequestMessage reqMsg;

            int msgId = message.DetermineMessageId().GetValueOrDefault();

            if(sentMessages.TryGetValue(msgId, out reqMsg))
            {
                sentMessages.Remove(msgId);
                reqMsg.Complete(message, null);
            }

            ResultReceived?.Invoke(this, new ResultReceivedEventArgs(message));
        }

        private void OnStateChanged(EventResult eventResult)
        {
            StateChanged?.Invoke(this, new StateChangedEventArgs(eventResult));

        }


    }
}