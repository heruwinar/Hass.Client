using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hass.Client.HassApi
{
    public class StateChangedEventArgs : EventArgs
    {

        public StateChangedEventArgs(EventResult @event)
        {
            Event = @event;
        }

        public EventResult Event { get; private set; }

    }

}
