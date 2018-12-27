using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Hass.Client.Models;
using Hass.Client.Models.Components;

namespace Hass.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public IComponent Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new UnkownEntity("unkown");

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopModalAsync();
        }
    }
}