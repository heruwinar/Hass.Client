using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Hass.Client.Models;
using Hass.Client.ViewModels;

namespace Hass.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ItemDetailPage()
        {
            InitializeComponent();

            var item = new Hass.Client.Models.Components.UnkownEntity("unknown");

            viewModel = new ItemDetailViewModel(item);
            BindingContext = viewModel;
        }
    }
}