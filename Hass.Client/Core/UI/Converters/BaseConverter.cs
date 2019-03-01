using System;
using System.Globalization;
using Xamarin.Forms;

namespace Hass.Client.Views.Converters
{ 

    public abstract class BaseConverter<TValue> : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert((TValue)value, parameter);
        }

        protected abstract object Convert(TValue value, object parameter);


        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
