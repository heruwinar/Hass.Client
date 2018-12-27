using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;


namespace Hass.Client.HassApi
{
    public class ResponseMessage : ApiMessage
    {
        public enum MessageType
        {
            Auth_required,
            Auth_ok,
            Auth_invalid,
            Result,
            Event
        }

        protected ResponseMessage(int? id, MessageType type)
            : base(id)
        {
            Type = type;
        }

        public static ResponseMessage Parse(string json)
        {
            return Parse(null, JObject.Parse(json));
        }

        public static ResponseMessage Parse(WsAPI context, JObject jsonObj)
        {
            string tp = jsonObj.GetValue<string>("type");

            if(string.IsNullOrEmpty(tp))
            {
                throw new ArgumentException($"invalid response format");
            }

            MessageType msgTp = (MessageType)Enum.Parse(typeof(MessageType), tp, true);

            ResponseMessage responseMsg;

            if (msgTp == MessageType.Auth_invalid
                || msgTp == MessageType.Auth_ok
                || msgTp == MessageType.Auth_required)
            {
                responseMsg = new AuthResponseMessage(msgTp);
                responseMsg.IsSuccess = msgTp == MessageType.Auth_invalid;
            }
            else
            {
                int? id = jsonObj.GetValue<int?>("id");

                responseMsg = new ResponseMessage(id, msgTp);

                responseMsg.IsSuccess = jsonObj.GetValue<bool?>("success");

                JObject errObj = jsonObj.GetValue<JObject>("error");

                if (errObj != null)
                {
                    responseMsg.Error = new Error(
                        errObj.GetValue<int>("error"),
                        errObj.GetValue<string>("message"));
                }
            }

            responseMsg.RequestMessage = context?.FindRequestMessageOrDefault(responseMsg.Id.GetValueOrDefault());

            if (responseMsg.RequestMessage?.Type == RequestMessage.MessageType.Get_states)
            {
                responseMsg.States = jsonObj
                    .GetValue<JObject[]>("result")
                    ?.Select(state => StateResult.Parse(state))
                    .ToArray();

                responseMsg.Result = responseMsg.States;
            }
            else if (responseMsg.RequestMessage?.Type == RequestMessage.MessageType.Subscribe_events)
            {
                responseMsg.Event = EventResult.Parse(jsonObj.GetValue<JObject>("event"));
                responseMsg.Result = responseMsg.Event;
            }
            else
            {
                responseMsg.Result = jsonObj.GetValue<object>("result");
            }

            return responseMsg;
        }

        public MessageType Type { get; private set; }

        public RequestMessage.MessageType RequestMessageType
        {
            get
            {
                return RequestMessage.Type;
            }
        }

        public RequestMessage RequestMessage { get; private set; }

        public bool? IsSuccess { get; private set; }
        
        public object Result { get; private set; }

        public EventResult Event { get; private set; }

        public StateResult[] States { get; private set; }

        public Error Error { get; private set; }
    }

}
