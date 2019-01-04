using System;
using System.Collections.Generic;
using System.Linq;
using Hass.Client.Models.Components;

namespace Hass.Client.Views.Converters
{

    public class IsEmptyConverter : BaseConverter<IEnumerable<object>>
    {

        public static IsEmptyConverter IsEmpty = new IsEmptyConverter();

        public static IsEmptyConverter IsNotEmpty = new IsEmptyConverter { IsNegate = true };

        public bool IsNegate { get; set; }

        protected override object Convert(IEnumerable<object> value,  object parameter)
        {
            bool v = value == null || !value.Any();
            return IsNegate ? !v : v;
        }

    }

}
