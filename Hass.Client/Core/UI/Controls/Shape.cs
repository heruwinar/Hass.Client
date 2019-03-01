using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Svg = SkiaSharp.Extended.Svg;

namespace Hass.Client.Views.Controls
{
    public abstract class Shape: SKCanvasView
    {

        public static BindableProperty StrokeWidthProperty = BindableProperty.Create(
            "StrokeWidth",
            typeof(float),
            typeof(Shape),
            defaultValue: 1.0f,
            propertyChanged: (s, o, n) => ((Shape)s).InvalidateSurface());

        public static BindableProperty StrokeColorProperty = BindableProperty.Create(
            "StrokeColor",
            typeof(Color),
            typeof(Shape),
            defaultValue: Color.Beige,
            propertyChanged: (s, o, n) => ((Shape)s).InvalidateSurface());

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

        protected sealed override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            var canvas = e.Surface.Canvas;

            canvas.Clear(SKColors.Transparent);

            SKSize size = canvas.DeviceClipBounds.Size;

            var paint = new SKPaint
            {
                IsAntialias = true,
                DeviceKerningEnabled = true,
                Color = StrokeColor.ToSKColor(),
                IsStroke = true,
                StrokeWidth = StrokeWidth,
            };

            OnPaint(canvas, ref size, ref paint);
        }

        protected abstract void OnPaint(SKCanvas canvas, ref SKSize size, ref SKPaint paint);
    }

}
