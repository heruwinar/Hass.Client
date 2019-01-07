using System;

namespace Hass.Client.Core
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

        public IViewModelNavigation Naviation
        {
            get
            {
                return ((IViewModel)this).Navigation ?? ShellContext.Navigation;
            }
        }

        IViewModelNavigation IViewModel.Navigation
        {
            get;
            set;
        }

    }

}
