using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Hass.Client.Views.Common
{
    public class BindingContextSpy: View
    {

        public BindingContextSpy():base()
        {

        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }

    }
}
