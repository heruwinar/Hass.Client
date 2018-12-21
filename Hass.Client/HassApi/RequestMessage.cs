using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hass.Client.Json;

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

        public RequestMessage(MessageType type, object requestObj = null)
            : base(null)
        {
            Type = type;
            RequestObject = requestObj;
        }

        public MessageType Type { get; private set; }

        public object RequestObject { get; set; }

        protected virtual object BuildRequestObject()
        {
            return RequestObject;
        }

        public string BuildRequestJson(int? messageId)
        {
            Id = messageId;

            JsonObject json = JsonObject.Create(BuildRequestObject()) ?? new JsonObject();

            if(Id != null)
            {
                json["id"] = Id.Value;
            }
            json["type"] = Type.ToString().ToLower();
            return json.Stringify();
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

    }
}
