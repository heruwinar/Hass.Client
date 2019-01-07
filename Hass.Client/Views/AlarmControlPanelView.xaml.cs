using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Hass.Client.Core;
using Hass.Client.ViewModels.Components;

namespace Hass.Client.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlarmControlPanelView : ContentView, IView<AlarmControlPanelViewModel>
    {
        public AlarmControlPanelView()
        {
            InitializeComponent();
        }

        IViewModel IView.ViewModel
        {
            get
            {
                return (IViewModel)BindingContext;
            }
            set
            {
                BindingContext = value;
            }
        }
    }

}