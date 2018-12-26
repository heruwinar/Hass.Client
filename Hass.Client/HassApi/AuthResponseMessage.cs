using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hass.Client.HassApi
{
    public class AuthResponseMessage : ResponseMessage
    {

        public const int DEFAULT_MESSAGEID = -11;

        public AuthResponseMessage(MessageType type)
            : base(DEFAULT_MESSAGEID, type)
        {
        }

    }
}
