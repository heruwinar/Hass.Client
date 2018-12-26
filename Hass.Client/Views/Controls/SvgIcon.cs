using System;
using System.Linq;
using System.IO;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Svg = SkiaSharp.Extended.Svg;

namespace Hass.Client.Views.Controls
{
    public class SvgIcon : SKCanvasView
    {
        public static BindableProperty SvgResourceKeyProperty = BindableProperty.Create(
            "SvgResourceKey",
            typeof(string),
            typeof(SvgIcon),
            null,
            BindingMode.OneWay,
            null,
            (s, old, @new) => ((SvgIcon)s).OnSvgResourceKeyChanged((string)old, (string)@new));

        private SKPicture picture;

        public string SvgResourceKey
        {
            get
            {
                return (string)GetValue(SvgResourceKeyProperty);
            }
            set
            {
                SetValue(SvgResourceKeyProperty, value);
            }
        }

        private void DisposePicture()
        {
            if(picture != null)
            {
                picture.Dispose();
                picture = null;
            }
        }

        private void OnSvgResourceKeyChanged(string oldValue, string newValue)
        {
            DisposePicture();
            //"XApp.Views.Icons.ZWave.svg"
            if (!string.IsNullOrEmpty(newValue))
            {
                string resxId = newValue.IndexOf(".") > 0 
                    ? newValue 
                    : $"{typeof(SvgIcon).Namespace}.Svg.{newValue}.svg";

                using (Stream stream = GetType().Assembly.GetManifestResourceStream(resxId))
                {
                    Svg.SKSvg svg = new Svg.SKSvg();
                    svg.Load(stream);
                    picture = svg.Picture;
                }
            }
            InvalidateSurface();
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);
            
            var canvas = e.Surface.Canvas;
            if(BackgroundColor.A > 0.1)
            {
                canvas.Clear(new SKColor(
                    (byte)(255* BackgroundColor.R), 
                    (byte)(255 * BackgroundColor.G), 
                    (byte)(255 * BackgroundColor.B), 
                    (byte)(255 * BackgroundColor.A)));
            }
            
            if(picture != null)
            {
                SKSize size = canvas.DeviceClipBounds.Size;
                SKSize pictureSz = picture.CullRect.Size;

                SKMatrix matrix = SKMatrix.MakeScale(size.Width / pictureSz.Width, size.Height / pictureSz.Height);

                canvas.DrawPicture(picture, ref matrix, new SKPaint
                {
                    IsAntialias = true
                });

            }
        }
        public static string[] ListAllEmbeddedSvgs()
        {
            string rootNs = typeof(SvgIcon).Namespace + ".Svg.";

            return typeof(SvgIcon).Assembly.GetManifestResourceNames()
                .Where(resx => resx.StartsWith(rootNs))
                .Where(resx => resx.EndsWith(".svg"))
                .Select(resx => Path.GetFileNameWithoutExtension(resx.Substring(rootNs.Length)))
                .ToArray();
        }

    }

}
