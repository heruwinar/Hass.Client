using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hass.Client.HassApi
{
    public class ResultReceivedEventArgs : EventArgs
    {

        public ResultReceivedEventArgs(ResponseMessage responseMessage)
        {
            ResponseMessage = responseMessage;
        }

        public int Id
        {
            get
            {
                return ResponseMessage.Id.GetValueOrDefault();
            }
        }

        public RequestMessage.MessageType? RequestMessageType
        {
            get
            {
                return RequestMessage?.Type;
            }
        }

        public RequestMessage RequestMessage
        {
            get
            {
                return ResponseMessage.RequestMessage;
            }
        }

        public ResponseMessage ResponseMessage { get; private set; }

    }

}
