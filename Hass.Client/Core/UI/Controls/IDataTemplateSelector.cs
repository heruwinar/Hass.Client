using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Hass.Client.Views.Controls
{
    public interface IDataTemplateSelector
    {
        DataTemplate SelectTemplate(object item, BindableObject container);
    }
}
