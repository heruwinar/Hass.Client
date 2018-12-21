using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Hass.Client.HassApi.Websocket;
using Hass.Client.Json;

namespace Hass.Client.HassApi
{
    public class WsAPI
    {
        private event EventHandler Authenticated;

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

        public int NextId()
        {
            return ++currentId;
        }

        public async Task<bool> ConnectAsync()
        {
            return await ConnectTask();
        }

        public Task<bool> ConnectTask()
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
            ws.ConnectAsync(Endpoint.ToString());
            return tcs.Task;
        }

        private void OnWsMessageReceived(
            object sender,
            MessageWebSocketMessageReceivedEventArgs args)
        {
            ProcessResponse(JsonObject.Parse(args.Message));
        }

        private void ProcessResponse(JsonObject responseJson)
        {
            var response = ResponseMessage.Parse(this, responseJson);
            switch (response.Type)
            {
                case ResponseMessage.MessageType.Auth_required:
                    SendMessageAsync(RequestMessage.CreateAuthMessage(APIPassword));
                    break;
                case ResponseMessage.MessageType.Auth_invalid:
                    OnAuthenticated();
                    break;
                case ResponseMessage.MessageType.Auth_ok:
                    IsAuthenticated = true;
                    OnAuthenticated();
                    break;
                case ResponseMessage.MessageType.Event:
                    OnStateChanged(response.Event);
                    break;
                case ResponseMessage.MessageType.Result:
                    OnResultReceived(response);
                    break;
            }

        }

        public Uri Endpoint { get; private set; }

        public string APIPassword { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public async Task SendMessageAsync(RequestMessage message)
        {
            int? msgId = message.Type == RequestMessage.MessageType.Auth
                ? null
                : new int?(NextId());

            string json = message.BuildRequestJson(msgId);

            if (msgId != null)
            {
                sentMessages.Add(message.Id.Value, message);
            }

            await ws.SendAsync(json);
        }



        public RequestMessage GetRequestMessage(int requestId)
        {
            if(requestId == subscribeEventMessage.Id)
            {
                return subscribeEventMessage;
            }
            RequestMessage req;
            sentMessages.TryGetValue(requestId, out req);
            return req;
        }

        public async Task<StateResult[]> ListStatesAsync()
        {
            return await ListStatesTask();
        }

        public Task<StateResult[]> ListStatesTask()
        {
            TaskCompletionSource<StateResult[]> tcs = new TaskCompletionSource<StateResult[]>();

            EventHandler<ResultReceivedEventArgs> handler = null;
            handler = (s, e) =>
            {
                if (e.RequestMessageType == RequestMessage.MessageType.Get_states)
                {
                    ResultReceived -= handler;
                    if (e.ResponseMessage.IsSuccess == true)
                    {
                        tcs.SetResult(e.ResponseMessage.States);
                    }
                    else
                    {
                        tcs.TrySetException(new Exception(e.ResponseMessage.Error?.ToString() ?? "An error has occured!"));
                    }
                }
            };

            ResultReceived += handler;

            SendMessageAsync(new RequestMessage(RequestMessage.MessageType.Get_states));

            return tcs.Task;
        }

        private void OnAuthenticated()
        {
            if(IsAuthenticated)
            {
                SendMessageAsync(subscribeEventMessage);
            }
            Authenticated?.Invoke(this, EventArgs.Empty);
        }

        private void OnResultReceived(ResponseMessage message)
        {
            ResultReceived?.Invoke(this, new ResultReceivedEventArgs(message));

            if (message.Id != null && sentMessages.ContainsKey(message.Id.Value))
            {
                sentMessages.Remove(message.Id.Value);
            }
        }

        private void OnStateChanged(EventResult eventResult)
        {
            StateChanged?.Invoke(this, new StateChangedEventArgs(eventResult));

        }

    }
}