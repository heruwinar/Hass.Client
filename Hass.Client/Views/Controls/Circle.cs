using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Svg = SkiaSharp.Extended.Svg;

namespace Hass.Client.Views.Controls
{
    public class Circle: SKCanvasView
    {

        public static BindableProperty StrokeWidthProperty = BindableProperty.Create(
            "StrokeWidth",
            typeof(float),
            typeof(Circle),
            defaultValue: 1.0f,
            propertyChanged: (s, o, n) => ((Circle)s).InvalidateSurface());

        public static BindableProperty StrokeColorProperty = BindableProperty.Create(
            "StrokeColor",
            typeof(Color),
            typeof(Circle),
            defaultValue: new Color(0, 0, 0),
            propertyChanged: (s, o, n) => ((Circle)s).InvalidateSurface());

        public float StrokeWidth
        {
            get { return (float)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        public Color StrokeColor
        {
            get { return (Color)GetValue(StrokeColorProperty); }
            set { SetValue(StrokeColorProperty, value); }
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            var canvas = e.Surface.Canvas;

            SKSize size = canvas.DeviceClipBounds.Size;

            canvas.DrawCircle(
                size.Width / 2, 
                size.Height / 2, 
                Math.Max(0, Math.Min(size.Width, size.Height) -2) / 2, new SKPaint
                {
                    IsAntialias = true,
                    DeviceKerningEnabled = true,
                    Color = StrokeColor.ToSKColor(),
                    IsStroke = true,
                    StrokeWidth = StrokeWidth,
                });
        }

    }

}
