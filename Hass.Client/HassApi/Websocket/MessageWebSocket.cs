using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Websockets;
using Hass.Client.Util;

namespace Hass.Client.HassApi.Websocket
{
    public sealed class MessageWebSocket
    {

        public event EventHandler<MessageWebSocketMessageReceivedEventArgs> MessageReceived;

        private IWebSocketConnection connection;

        public void ConnectAsync(string endpoint)
        {
            connection = WebSocketFactory.Create();
            connection.Open(endpoint);
            connection.OnMessage += Connection_OnMessage;
        }

        private void Connection_OnMessage(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                MessageReceived.Invoke(this, new MessageWebSocketMessageReceivedEventArgs(msg));
            }
        }

        public async Task SendAsync(string msg)
        {
            await SendTask(msg);
        }

        public Task SendTask(string msg)
        {
            return Task.Factory.StartNew(
                () => connection.Send(msg),
                CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

    }
}
