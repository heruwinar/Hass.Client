using System;
using System.Collections;
using System.Linq;
using Hass.Client.Models.Components;

namespace Hass.Client.Views.Converters
{

    public class IsEmptyConverter : BaseConverter<IEnumerable>
    {

        public static IsEmptyConverter IsEmpty = new IsEmptyConverter();

        public static IsEmptyConverter IsNotEmpty = new IsEmptyConverter { IsNegate = true };

        public bool IsNegate { get; set; }

        protected override object Convert(IEnumerable value,  object parameter)
        {
            bool v = value == null || !value.GetEnumerator().MoveNext();
            return IsNegate ? !v : v;
        }

    }

}
