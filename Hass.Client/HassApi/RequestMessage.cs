using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Hass.Client.HassApi
{
    public class RequestMessage : ApiMessage
    {
        public enum MessageType
        {
            Auth,
            Subscribe_events,
            Unsubscribe_events,
            Call_service,
            Get_states,
            Get_config,
            Get_services,
            Get_panels,
            Camera_thumbnail,
            Media_player_thumbnail,
        }

        private TaskCompletionSource<ResponseMessage> taskCompleteSource = new TaskCompletionSource<ResponseMessage>();

        public RequestMessage(MessageType type, object requestObj = null)
            : base(null)
        {
            Type = type;
            RequestObject = requestObj;
            CreatedOn = DateTime.Now;
        }


        public MessageType Type { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public object RequestObject { get; set; }

        protected virtual object BuildRequestObject()
        {
            return RequestObject;
        }

        public override int? DetermineMessageId()
        {
            return Type == MessageType.Auth 
                ? new int?(AuthResponseMessage.DEFAULT_MESSAGEID) 
                : base.DetermineMessageId();
        }

        public string BuildRequestJson(int? messageId)
        {
            Id = messageId;

            JObject json = JObject.FromObject(BuildRequestObject() ?? new object());

            if(Id != null)
            {
                json["id"] = Id.Value;
            }

            json["type"] = Type.ToString().ToLower();

            string stringify = json.ToString();
            return stringify;
        }

        public static RequestMessage CreateAuthMessage(string api_password)
        {
            return new RequestMessage(MessageType.Auth, new { api_password = api_password });
        }

        public static RequestMessage CreateSubscribeStateChangeEventMessage()
        {
            return new RequestMessage(MessageType.Subscribe_events, new { event_type = "state_changed" });
        }

        public static RequestMessage CreateCallServiceMessage(string domain, string service, object serviceData)
        {
            return new RequestMessage(
                MessageType.Call_service,
                new { domain = domain, service = service, service_data = serviceData });
        }

        public Task<ResponseMessage> Task
        {
            get
            {
                return taskCompleteSource.Task;
            }
        }

        public void Complete(ResponseMessage response, Exception error)
        {
            if (response != null)
            {
                taskCompleteSource.SetResult(response);
            }
            if (error != null)
            {
                taskCompleteSource.SetException(error);
            }
        }

    }
}
