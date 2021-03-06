﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Hass.Client.Models;
using Hass.Client.ViewModels;

namespace Hass.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            iconsPicker.ItemsSource = Controls.SvgIcon.ListAllEmbeddedSvgs();

            iconsPicker.SelectedIndexChanged += (s, e) => iconCtrl.SvgResourceKey = (string)iconsPicker.SelectedItem;

            BindingContext = viewModel = new ItemsViewModel();


        }

        //async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        //{
        //    var item = args.SelectedItem as IComponent;
        //    if (item == null)
        //        return;

        //    await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

        //    // Manually deselect item.
        //    ItemsListView.SelectedItem = null;
        //}

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        //private async void ItemsView_Selected(object sender, EventArgs e)
        //{
        //    var item = ((Controls.ItemsView)sender).SelectedItem as IComponent;
        //    if (item != null)
        //    {
        //        var alarm = item as Models.Components.AlarmControlPanel;
        //        if(alarm != null)
        //        {
        //            alarmPanel.BindingContext = new ViewModels.Components.AlarmControlPanelViewModel(alarm);
        //        }
        //        await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
        //    }
        //}
    }
}