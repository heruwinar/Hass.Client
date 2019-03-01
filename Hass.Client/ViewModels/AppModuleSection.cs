using System;
using System.Collections.Generic;
using Hass.Client.Core;
using Hass.Client.Models;

namespace Hass.Client.ViewModels
{
    public abstract class AppModuleSection : ModelBase, IAppModuleSection
    {

        private string title;

        private IAppModuleItem selectedItem;

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                SetProperty(ref title, value);
            }
        }

        public IEnumerable<IAppModuleItem> Items => throw new NotImplementedException();

        public IAppModuleItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                SetProperty(ref selectedItem, value);
            }
        }

    }
}
