using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hass.Client.Core;
using Hass.Client.Models;
using Hass.Client.ViewModels.Product;
using Hass.Client.ViewModels.Scene;

namespace Hass.Client.ViewModels
{
    public class AppShell: ViewModelBase
    {

        private ObservableCollection<IAppModule> appModules;

        private IAppModule selectedAppModule;

        public AppShell()
        {
            appModules = new ObservableCollection<IAppModule>
            {
                new ProductModule(),
                new SceneModule(),
            };
            selectedAppModule = appModules[1];
        }

        public IEnumerable<IAppModule> AppModules => appModules;


        public IAppModule SelectedAppModule
        {
            get
            {
                return selectedAppModule;
            }
            set
            {
                SetProperty(ref selectedAppModule, value);
            }
        }

    }

}
