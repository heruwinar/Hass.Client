using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Hass.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void TabbedPage_CurrentPageChanged(object sender, EventArgs e)
        {
            
        }
    }
}