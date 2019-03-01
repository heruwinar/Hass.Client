using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Hass.Client.Models;
using Hass.Client.Core;

namespace Hass.Client.ViewModels
{
    public class ItemsViewModel : ViewModelBase
    {
        private HassSystem hass = new HassSystem();
        private IComponent selectedComponent;

        public ItemsViewModel()
        {
            Title = "Products";
            Items = new ObservableCollection<IComponent>();
            LoadItemsCommand = new BindableCommand(async () => await ExecuteLoadItemsCommand());
            SelectItemCommand = new BindableCommand<IComponent>(SelectItem);
        }

        public BindableCommand<IComponent> SelectItemCommand { get; private set; }

        public IComponent SelectedComponent
        {
            get
            {
                return selectedComponent;
            }
            set
            {
                SetProperty(ref selectedComponent, value);
            }
        }
        
        public ObservableCollection<IComponent> Items { get; set; }

        public string Name
        {
            get
            {
                return "Abc";
            }
        }


        public BindableCommand LoadItemsCommand { get; set; }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                await hass.Initialize();
                var items = new ObservableCollection<IComponent>();
                foreach (IComponent c in hass.AllEntities)
                {
                    items.Add(c);
                }
                ShellContext.BeginInvokeOnMainThread(()=>
                    {
                        Items = items;
                        OnPropertyChanged(nameof(Items));
                        SelectedComponent = items.FirstOrDefault();
                    });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void SelectItem(IComponent item)
        {
            var alarm = item as Models.Components.AlarmControlPanel;
            if (alarm != null)
            {
                var alarmVm = new ViewModels.Components.AlarmControlPanelViewModel(alarm);
                Naviation.PushAsync(alarmVm);
            }
        }
    }
}