using System;
using System.Collections.Generic;
using System.Text;

namespace Hass.Client.HassApi.Websocket
{
    public class MessageWebSocketMessageReceivedEventArgs : EventArgs
    {

        public MessageWebSocketMessageReceivedEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}
