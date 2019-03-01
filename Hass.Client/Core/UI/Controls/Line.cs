using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Svg = SkiaSharp.Extended.Svg;

namespace Hass.Client.Views.Controls
{
    public class Line: Shape
    {

        protected override void OnPaint(SKCanvas canvas, ref SKSize size, ref SKPaint paint)
        {
            float y = size.Height / 2;
            canvas.DrawLine(0, y, size.Width, y, paint);
        }

    }

}
