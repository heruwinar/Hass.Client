using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Hass.Client.Models;
using Hass.Client.Views;

namespace Hass.Client.ViewModels
{
    public class ItemsViewModel : ViewModelBase
    {
        private HassSystem hass = new HassSystem();
        private IComponent selectedComponent;

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


        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<IComponent>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, IComponent>(this, "AddItem",  (obj, item) =>
            {
            });
            ExecuteLoadItemsCommand();
        }

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
                Device.BeginInvokeOnMainThread(()=>
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
    }
}