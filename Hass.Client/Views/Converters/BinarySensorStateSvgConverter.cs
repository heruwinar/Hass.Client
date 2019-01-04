using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using Hass.Client.Models.Components;

namespace Hass.Client.Views.Converters
{
    public class BinarySensorStateSvgConverter : BaseConverter<bool>
    {

        public static BinarySensorStateSvgConverter Default = new BinarySensorStateSvgConverter();

        protected override object Convert(bool value, object parameter)
        {
            return value ? "binary-sensor-on" : "binary-sensor-off";
        }

    }

}
