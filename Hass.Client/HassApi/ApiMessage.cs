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

        protected int? Id { get; set; }

        public virtual int? DetermineMessageId()
        {
            return Id;
        }

    }
}
