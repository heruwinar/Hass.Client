using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Hass.Client.Models;
using Hass.Client.ViewModels;

namespace Hass.Client.Views.Products
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductGroupsPage : ContentPage
    {
        public ProductGroupsPage()
        {
            InitializeComponent();
            BindingContextChanged += ProductGroupsPage_BindingContextChanged;
        }

        private void ProductGroupsPage_BindingContextChanged(object sender, EventArgs e)
        {
        }
    }
}