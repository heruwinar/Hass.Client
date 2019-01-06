using System;
using System.Collections.Generic;
using System.Text;

namespace Hass.Client.Views.Converters
{
    public class IsNotNullConverter: BaseConverter<object>
    {

        public static IsNotNullConverter Default = new IsNotNullConverter();

        protected override object Convert(object value, object parameter)
        {
            return value != null;
        }

    }
}
