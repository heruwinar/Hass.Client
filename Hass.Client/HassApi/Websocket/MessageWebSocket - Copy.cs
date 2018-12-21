//using System;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Threading;
//using System.Net.WebSockets;
//using Hass.Client.Util;

//namespace Hass.Client.HassApi.Websocket
//{
//    public sealed class MessageWebSocket
//    {

//        public event EventHandler<MessageWebSocketMessageReceivedEventArgs> MessageReceived;

//        private ClientWebSocket client;
//        private CancellationToken cts;

//        public async void ConnectAsync(Uri endpoint)
//        {
//            cts = new CancellationToken();
//            client = new ClientWebSocket();
//            await client.ConnectAsync(new Uri("ws://lockng.duckdns.org/api/websocket"), cts);
//            Task.Factory.StartNew(ReceiveMessages, cts, TaskCreationOptions.LongRunning, TaskScheduler.Default);
//        }

//        public async Task SendAsync(string json)
//        {
//            var array = new ArraySegment<byte>(Encoding.UTF8.GetBytes(json));
//            await client.SendAsync(array, WebSocketMessageType.Text, true, CancellationToken.None);
//        }

//        private void ReceiveMessages()
//        {
//            while(true)
//            {
//                string msg = ReadMessage();
//                if(!string.IsNullOrEmpty(msg))
//                {
//                    MessageReceived.Invoke(this, new MessageWebSocketMessageReceivedEventArgs(msg));
//                }
//            }
//        }

//        private string ReadMessage()
//        {
//            var sb = new StringBuilder();
//            var message = new ArraySegment<byte>(new byte[8192]);
//            while(true)
//            {
//                WebSocketReceiveResult result = client.ReceiveAsync(message, cts).Await();
//                if (result.MessageType != WebSocketMessageType.Text)
//                {
//                    return null;
//                }
//                var messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
//                string msgFragement = Encoding.UTF8.GetString(messageBytes);
//                Console.WriteLine("Received: {0}", msgFragement);
//                sb.Append(msgFragement);
//                if(result.EndOfMessage)
//                {
//                    break;
//                }
//            }
//            return sb.ToString();
//        }

//    }
//}
