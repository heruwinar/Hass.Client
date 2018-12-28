using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Hass.Client.Views.Controls
{
    public class ItemContainer: Frame
    {

        public static BindableProperty IsSelectedProperty = BindableProperty.Create(
            "IsSelected", 
            typeof(bool), 
            typeof(ItemContainer), 
            false,
            BindingMode.TwoWay,
            propertyChanged: (s, o, n)=> ((ItemContainer)s).OnIsSelectedChanged((bool)n));

        private ItemsView itemsView;

        public ItemContainer(ItemsView itemsView)
        {
            this.itemsView = itemsView;
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        private void OnIsSelectedChanged(bool selected)
        {
            itemsView.ProcessItemIsSelectedChanged(this);
        }

    }

}
