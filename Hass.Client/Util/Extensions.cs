using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Net.WebSockets;

namespace Hass.Client.Util
{
    public static class Extensions
    {

        public static void Await(this Task task)
        {
            task.Wait();
            if (task.IsFaulted)
            {
                throw task.Exception;
            }
        }

        public static T Await<T>(this Task<T> task)
        {
            task.Wait();
            if (task.IsFaulted)
            {
                throw task.Exception;
            }
            return task.Result;
        }

    }

}
