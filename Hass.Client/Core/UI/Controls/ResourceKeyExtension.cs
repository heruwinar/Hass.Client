using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Hass.Client.Views.Controls
{

    [ContentProperty("Type")]
    public class ResourceKeyExtension : IMarkupExtension<string>
    {

        public Type Type { get; set; }

        public string ProvideValue(IServiceProvider serviceProvider)
        {
            return Type?.FullName ?? "AKey";
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return Type?.FullName ?? "AKey";
        }
    }
}
