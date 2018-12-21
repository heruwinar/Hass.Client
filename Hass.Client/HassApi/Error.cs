using System;
using System.Collections.Generic;
using System.Linq;

namespace Hass.Client.HassApi
{
    public class Error
    {
        public Error(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; private set; }

        public string Message { get; private set; }

        public override string ToString()
        {
            return $"code: {Code}, message: {Message}"; 
        }

    }
}
