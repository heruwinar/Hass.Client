using System;
using Hass.Client.Core;
using Hass.Client.Models;

namespace Hass.Client.ViewModels
{
    public class ItemDetailViewModel : ViewModelBase
    {
        public IComponent Item { get; set; }

        public ItemDetailViewModel(IComponent item = null)
        {
            Title = item?.Platform.ToString();
            Item = item;
        }
    }
}
