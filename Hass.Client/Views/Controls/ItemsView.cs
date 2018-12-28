using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace Hass.Client.Views.Controls
{

    public class ItemsView : ContentView
    {

        public static readonly BindableProperty LayoutProperty = BindableProperty.Create(
            "Layout",
            typeof(Layout<View>),
            typeof(ItemsView),
            null,
            propertyChanged: (bindable, oldvalue, newvalue) =>
                ((ItemsView)bindable).OnLayoutChanged((Layout<View>)oldvalue, (Layout<View>)newvalue));

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            "ItemsSource",
            typeof(IEnumerable),
            typeof(ItemsView),
            null,
            propertyChanged: (bindable, oldvalue, newvalue) =>
                ((ItemsView)bindable).OnItemsSourceChanged((IEnumerable)oldvalue, (IEnumerable)newvalue));

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
            "ItemTemplate",
            typeof(DataTemplate),
            typeof(ItemsView),
            propertyChanged: (bindable, oldvalue, newvalue) =>((ItemsView)bindable).RebuildChildren());

        public static readonly BindableProperty ItemTemplateSelectorProperty = BindableProperty.Create(
            "ItemTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(ItemsView),
            propertyChanged: (bindable, oldvalue, newvalue) =>((ItemsView)bindable).RebuildChildren());


        public ItemsView()
        {
            Layout = new StackLayout();
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public new Layout<View> Layout
        {
            get { return (Layout<View>)GetValue(LayoutProperty); }
            set { SetValue(LayoutProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public DataTemplateSelector ItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        private void OnLayoutChanged(Layout<View> oldValue, Layout<View> newValue)
        {
            Content = null;
            if(oldValue != null)
            {
                oldValue.Children.Clear();
            }
            Content = newValue;
            RebuildChildren();
        }

        private void RebuildChildren()
        {
            if(Layout != null)
            {
                Layout.Children.Clear();
                BuildChildren(ItemsSource, 0);
            }
        }

        private void BuildChildren(IEnumerable items, int startingIndex)
        {
            IList<View> children = Layout?.Children;
            
            if(children != null && items != null)
            {
                DataTemplateSelector templateSel = ItemTemplateSelector;
                if (templateSel != null)
                {
                    foreach (object item in ItemsSource)
                    {
                        children.Insert(startingIndex++, CreateItemView(item, templateSel.SelectTemplate(item, this)));
                    }
                }
                else
                {
                    DataTemplate template = ItemTemplate;
                    foreach (object item in ItemsSource)
                    {
                        children.Insert(startingIndex++, CreateItemView(item, template));
                    }
                }
            }
        }

        private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            Action<INotifyCollectionChanged, bool> installEvtHandler = (col, install) =>
            {
                if(col != null)
                {
                    if(install)
                    {
                        col.CollectionChanged += OnItemsSourceCollectionChanged;
                    }
                    else
                    {
                        col.CollectionChanged -= OnItemsSourceCollectionChanged;
                    }
                }
            };
            installEvtHandler(oldValue as INotifyCollectionChanged, false);
            installEvtHandler(newValue as INotifyCollectionChanged, true);
            RebuildChildren();
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IList<View> children = Layout?.Children;
            if(children == null)
            {
                return;
            }
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    children.Clear();
                    break;
                case NotifyCollectionChangedAction.Add:
                    BuildChildren(e.NewItems, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (object item in e.OldItems)
                    {
                        children.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                default:
                    throw new NotImplementedException(e.Action.ToString());
            }
        }

        private View CreateItemView(object item, DataTemplate itemTemplate)
        {
            View itemView;
            
            if(itemTemplate == null)
            {
                itemView = new Label() { Text = item.ToString() };                
            }
            else
            {
                itemView = (View)itemTemplate.CreateContent();
            }
            itemView.BindingContext = item;
            return itemView;
        }

    }
}