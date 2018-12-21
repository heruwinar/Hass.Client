using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hass.Client.Json;


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

        public ResponseMessage(int? id, MessageType type)
            : base(id)
        {
            Type = type;
        }

        public static ResponseMessage Parse(WsAPI context, JsonObject jsonObj)
        {
            string tp = jsonObj.GetValue<string>("type");

            if(string.IsNullOrEmpty(tp))
            {
                throw new ArgumentException($"invalid response format");
            }

            MessageType msgTp = (MessageType)Enum.Parse(typeof(MessageType), tp, true);

            if (msgTp == MessageType.Auth_invalid
                || msgTp == MessageType.Auth_ok
                || msgTp == MessageType.Auth_required)
            {
                return new AuthResponseMessage(msgTp);
            }

            int? id = jsonObj.GetValue<int?>("id");

            var res = new ResponseMessage(id, msgTp);

            res.IsSuccess = jsonObj.GetValue<bool?>("success");

            JsonObject errObj = jsonObj.GetValue<JsonObject>("error");

            if (errObj != null)
            {
                res.Error = new Error(
                    errObj.GetValue<int>("error"),
                    errObj.GetValue<string>("message"));
            }

            res.RequestMessage = context.GetRequestMessage(id.GetValueOrDefault());

            if(res.RequestMessage?.Type == RequestMessage.MessageType.Get_states)
            {
                res.States = jsonObj
                    .GetValue<JsonObject[]>("result")
                    ?.Select(state => StateResult.Parse(state))
                    .ToArray();

                res.Result = res.States;
            }
            else if(res.RequestMessage?.Type == RequestMessage.MessageType.Subscribe_events)
            {
                res.Event = EventResult.Parse(jsonObj.GetValue<JsonObject>("event"));
                res.Result = res.Event;
            }
            else
            {
                res.Result = jsonObj.GetValue<object>("result");
            }
            return res;
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
