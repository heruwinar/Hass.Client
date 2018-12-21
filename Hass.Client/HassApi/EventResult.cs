using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hass.Client.Json;

namespace Hass.Client.HassApi
{

    public class EventResult
    {
        protected EventResult()
        {

        }

        public string EventType { get; private set; }

        public DateTime TimeFired { get; private set; }

        public string Origin { get; private set; }

        public EventData Data { get; private set; }

        public static EventResult Parse(JsonObject eventResultJson)
        {
            var evt = new EventResult();
            evt.EventType = eventResultJson.GetValue<string>("event_type");
            evt.Origin = eventResultJson.GetValue<string>("origin");
            evt.TimeFired = eventResultJson.GetValue<DateTime>("time_fired");

            JsonObject evtDataJson = eventResultJson.GetValue<JsonObject>("data");
            if(evtDataJson != null)
            {
                evt.Data = EventData.Parse(evtDataJson);
            }
            return evt;
        }

    }

    public class EventData
    {

        protected EventData()
        {
        }

        public string EntityId { get; private set; }

        public StateResult NewState { get; private set; }

        public StateResult OldState { get; private set; }

        public static EventData Parse(JsonObject dataJson)
        {
            var data = new EventData();

            data.EntityId = dataJson.GetValue<string>("entity_id");
            JsonObject oldState = dataJson.GetValue<JsonObject>("old_state");
            JsonObject newState = dataJson.GetValue<JsonObject>("new_state");

            if (oldState != null)
            {
                data.OldState = StateResult.Parse(oldState);
            }
            if (newState != null)
            {
                data.NewState = StateResult.Parse(newState);
            }

            return data;
        }
    }


}
