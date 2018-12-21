using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hass.Client.HassApi
{
    public class AuthResponseMessage : ResponseMessage
    {
        public AuthResponseMessage(MessageType type)
            : base(null, type)
        {
        }

    }
}
