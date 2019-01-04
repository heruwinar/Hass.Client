using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Net.WebSockets;
using Xamarin.Forms;
using SkiaSharp;

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

        public static void AnimateFontSize(this Label owner, string name, double fontSize, Action finished)
        {
            if(owner.FontSize == fontSize)
            {
                finished();
                return;
            }

            var anim = new Animation(v => owner.FontSize = v , start: owner.FontSize, end: fontSize);

            anim.Commit(
                owner,
                name,
                length: 150,
                easing: Easing.Linear, 
                finished: (a, f) =>
                {
                    owner.FontSize = fontSize;
                    finished();
                });
        }

    }

}
