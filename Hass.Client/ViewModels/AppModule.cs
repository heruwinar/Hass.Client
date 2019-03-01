using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hass.Client.Core;
using Hass.Client.Models;

namespace Hass.Client.ViewModels
{
    public class AppModule<TModuleSection> : ModelBase, IAppModule 
        where TModuleSection: class, IAppModuleSection
    {
        private string title;

        private ObservableCollection<TModuleSection> 
                    sections = new ObservableCollection<TModuleSection>();


        private TModuleSection selectedItem;

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

        public IEnumerable<IAppModuleSection> Items => (IEnumerable<IAppModuleSection>)sections;

        public TModuleSection SelectedItem
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

        IAppModuleSection IAppModule.SelectedItem
        {
            get
            {
                return SelectedItem;
            }
            set
            {
                SelectedItem = (TModuleSection)value;
            }
        }

        protected void AddSection(TModuleSection section)
        {
            sections.Add(section);
        }

    }

}
