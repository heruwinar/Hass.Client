using System.Collections.Generic;
using System.Linq;
using System.IO;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Svg = SkiaSharp.Extended.Svg;
using Hass.Client.Util;

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

        private static Dictionary<string, SKPicture> svgs = new Dictionary<string, SKPicture>();


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

        private void OnSvgResourceKeyChanged(string oldValue, string newValue)
        {
            //"XApp.Views.Icons.ZWave.svg"
            picture = null;

            if (!string.IsNullOrEmpty(newValue))
            {
                string resxId = newValue.IndexOf(".") > 0 
                    ? newValue 
                    : $"{typeof(SvgIcon).Namespace}.Svg.{newValue}.svg";

                if(!svgs.TryGetValue(resxId, out picture))
                {
                    using (Stream stream = GetType().Assembly.GetManifestResourceStream(resxId))
                    {
                        if(stream != null)
                        {
                            Svg.SKSvg svg = new Svg.SKSvg();
                            svg.Load(stream);
                            picture = svgs[resxId] = svg.Picture;
                        }
                    }
                }
            }
            InvalidateSurface();
        }

        
        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);
            
            var canvas = e.Surface.Canvas;
            //if(BackgroundColor.A > 0.1)
            //{
            //    canvas.Clear(BackgroundColor.ToSKColor());
            //}
            canvas.Clear(SKColors.Transparent);
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
                .OrderBy(resx => resx)
                .ToArray();
        }

    }

}
