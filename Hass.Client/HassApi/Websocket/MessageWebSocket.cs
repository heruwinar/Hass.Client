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
            connection.OnClosed += Connection_OnClosed;
            connection.OnMessage += Connection_OnMessage;
            connection.OnError += Connection_OnError;
        }

        private void Connection_OnError(string obj)
        {
        }

        private void Connection_OnClosed()
        {
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
            connection.Send(msg);
            return Task.CompletedTask;
            //return Task.Factory.StartNew(
            //    () => connection.Send(msg),
            //    CancellationToken.None,
            //    TaskCreationOptions.LongRunning,
            //    TaskScheduler.Default);
        }

    }
}
