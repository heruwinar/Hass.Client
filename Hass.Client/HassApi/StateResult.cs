using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hass.Client.Json;

namespace Hass.Client.HassApi
{
    public class StateResult
    {
        protected StateResult()
        {

        }

        public string EntityId { get; private set; }

        public string State { get; private set; }

        public DateTime LastChanged { get; private set; }

        public DateTime LastUpdated { get; private set; }

        public JsonObject Attributes { get; private set; }

        public JsonObject Context { get; private set; }

        public static StateResult Parse(JsonObject evtStateJson)
        {
            var state = new StateResult();
            state.EntityId = evtStateJson.GetValue<string>("entity_id");
            state.State = evtStateJson.GetValue<string>("state");
            state.LastChanged = evtStateJson.GetValue<DateTime>("last_changed");
            state.LastUpdated = evtStateJson.GetValue<DateTime>("last_updated");
            state.Attributes = evtStateJson.GetValue<JsonObject>("attributes");
            state.Context = evtStateJson.GetValue<JsonObject>("context");
            return state;
        }

    }
}
