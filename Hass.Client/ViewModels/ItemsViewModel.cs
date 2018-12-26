using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Hass.Client.Models;
using Hass.Client.Views;

namespace Hass.Client.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
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

                var wsClient = new HassApi.WsAPI(new Uri("ws://lockng.duckdns.org/api/websocket"), "l159456753");

                await wsClient.ConnectAsync();

                HassApi.StateResult[] states = await wsClient.ListStatesAsync();

                foreach (var state in states)
                {
                    Items.Add(new Item {Id = state.EntityId, Text = state.EntityId, Description = $"{state.State} - {state.Attributes}" });
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