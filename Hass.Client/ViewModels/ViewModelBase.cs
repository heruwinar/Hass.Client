using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Hass.Client.Models;

namespace Hass.Client.ViewModels
{
    public class ViewModelBase : ModelBase, IViewModel
    {

        private string title = string.Empty;
        private bool isBusy = false;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public IShellContext ShellContext
        {
            get
            {
                return App.Current;
            }
        }

    }

}
