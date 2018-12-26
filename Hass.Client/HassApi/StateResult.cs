using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

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

        public JObject Attributes { get; private set; }

        public JObject Context { get; private set; }

        public static StateResult Parse(JObject evtStateJson)
        {
            var state = new StateResult();
            state.EntityId = evtStateJson.GetValue<string>("entity_id");
            state.State = evtStateJson.GetValue<string>("state");
            state.LastChanged = evtStateJson.GetValue<DateTime>("last_changed");
            state.LastUpdated = evtStateJson.GetValue<DateTime>("last_updated");
            state.Attributes = evtStateJson.GetValue<JObject>("attributes");
            state.Context = evtStateJson.GetValue<JObject>("context");
            return state;
        }

    }
}
