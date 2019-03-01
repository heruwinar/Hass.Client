using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Svg = SkiaSharp.Extended.Svg;

namespace Hass.Client.Views.Controls
{
    public class Circle: Shape
    {

        protected override void OnPaint(SKCanvas canvas, ref SKSize size, ref SKPaint paint)
        {
            canvas.DrawCircle(
                size.Width / 2,
                size.Height / 2,
                Math.Max(0, Math.Min(size.Width, size.Height) - 2) / 2,
                paint);
        }

    }

}
