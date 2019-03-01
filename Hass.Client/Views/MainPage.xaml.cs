using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Hass.Client.ViewModels;

namespace Hass.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new AppShell();
        }
    }
}