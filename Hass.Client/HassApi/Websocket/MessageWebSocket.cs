using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Websockets;

namespace Hass.Client.HassApi.Websocket
{
    public sealed class MessageWebSocket
    {

        public event EventHandler<MessageWebSocketMessageReceivedEventArgs> MessageReceived;

        private IWebSocketConnection connection;

        public void Connect(string endpoint)
        {
            connection = WebSocketFactory.Create();
            connection.OnOpened += Connection_OnOpened;
            connection.OnClosed += Connection_OnClosed;
            connection.OnMessage += Connection_OnMessage;
            connection.OnError += Connection_OnError;
            connection.OnLog += Connection_OnLog;

            connection.Open(endpoint);
        }

        private void Connection_OnOpened()
        {
        }
        private void Connection_OnClosed()
        {
        }

        private void Connection_OnLog(string obj)
        {
        }

        private void Connection_OnError(string obj)
        {
        }

        private void Connection_OnMessage(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                MessageReceived.Invoke(this, new MessageWebSocketMessageReceivedEventArgs(msg));
            }
        }

        public void Send(string message)
        {
            connection.Send(message);
        }

    }
}
