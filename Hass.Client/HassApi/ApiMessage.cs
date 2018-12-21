using System;
using System.Collections.Generic;
using System.Linq;

namespace Hass.Client.HassApi
{
    public class ApiMessage
    {
        protected ApiMessage(int? id)
        {
            Id = id;
        }

        public int? Id { get; protected set; }

    }
}
