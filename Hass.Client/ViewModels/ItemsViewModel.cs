using System;
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

        public ObservableCollection<IComponent> Items { get; set; }

        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<IComponent>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, IComponent>(this, "AddItem",  (obj, item) =>
            {
            });
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
                foreach(IComponent c in hass.AllEntities)
                {
                    Items.Add(c);
                }
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