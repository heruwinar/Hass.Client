using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(Hass.Client.iOS.Renderers.ButtonRenderer))]

namespace Hass.Client.iOS.Renderers
{
    public class ButtonRenderer: Xamarin.Forms.Platform.iOS.ButtonRenderer
    {
        //public override void Draw(RectangleF rect)
        //{
        //    base.Draw(rect);
        //    UIButton btn = (UIButton)Control;
        //    btn.bor
        //    CAGradientLayer btnGradient = new CAGradientLayer();
        //    btnGradient.Frame = btn.Bounds;
        //    btnGradient.Colors = new CGColor[] { Color.White.ToCGColor(), Color.FromHex("#0073BD").ToCGColor() };
        //    btn.Layer.InsertSublayer(btnGradient, 0);
        //    btn.Layer.MasksToBounds = true;
        //    btn.Layer.BorderColor = Color.FromHex("#0073BE").ToCGColor();
        //    btn.Layer.BorderWidth = 1;
        //    btn.SetTitleColor(Color.Black.ToUIColor(), UIControlState.Normal);
        //}
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            Control.BackgroundColor = new UIColor(0.9f, 0.9f, 0.9f, 0.9f);
        }

    }


}