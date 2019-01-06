using System;
using System.Collections.Generic;
using System.Text;

namespace Hass.Client.Views.Converters
{
    public class IsNullConverter: BaseConverter<object>
    {

        public static IsNullConverter Default = new IsNullConverter();

        protected override object Convert(object value, object parameter)
        {
            return value == null;
        }

    }
}
