using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using Xamarin.Forms;
using Hass.Client.Core;
using Hass.Client.ViewModels;
using Hass.Client.Views.Controls;

namespace Hass.Client.Views.Behaviors
{
    public class TabbedPageBehavior : Behavior<TabbedPage>
    {

        private TabbedPage page;

        public static BindableProperty ItemsSourceProperty = BindableProperty.Create(
            "ItemsSource",
            typeof(IEnumerable),
            typeof(TabbedPageBehavior),
            default(IEnumerable),
            propertyChanged: (s, o, n) => 
                ((TabbedPageBehavior)s).OnItemsSourceChanged((IEnumerable)o, (IEnumerable)n));


        public static BindableProperty SelectedItemProperty = BindableProperty.Create(
            "SelectedItem",
            typeof(object),
            typeof(TabbedPageBehavior),
            default(object),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (s, o, n) => ((TabbedPageBehavior)s).OnSelectedItemChanged());

        public static BindableProperty ItemTemplateSelectorProperty = BindableProperty.Create(
            "ItemTemplateSelector",
            typeof(IDataTemplateSelector),
            typeof(TabbedPageBehavior),
            default(IDataTemplateSelector),
            propertyChanged: (s, o, n) => ((TabbedPageBehavior)s).OnItemTemplateSelectorChanged());

        public IDataTemplateSelector ItemTemplateSelector
        {
            get
            {
                return (IDataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
            }
            set
            {
                SetValue(ItemTemplateSelectorProperty, value);
            }
        }

        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public object SelectedItem
        {
            get
            {
                return (object)GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        private void OnSelectedItemChanged()
        {
            if(page != null)
            {
                object item = SelectedItem;
                if(page.CurrentPage?.BindingContext != item)
                {
                    page.CurrentPage = page.Children.FirstOrDefault(pg => pg.BindingContext == item);
                }
            }
        }

        private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            PerformTabBinding();
            InstallItemsSourceChangeEvent(oldValue as INotifyCollectionChanged, false);
            InstallItemsSourceChangeEvent(newValue as INotifyCollectionChanged, true);
        }

        private void OnItemTemplateSelectorChanged()
        {
            PerformTabBinding();
        }

        protected override void OnAttachedTo(TabbedPage page)
        {
            base.OnAttachedTo(page);

            this.page = page;
            SetBinding(
                BindingContextProperty, 
                new Binding(BindingContextProperty.PropertyName) {Source=page });
            page.CurrentPageChanged += OnTabbedPageCurrentPageChanged;
        }

        protected override void OnDetachingFrom(TabbedPage page)
        {
            base.OnDetachingFrom(page);
            page.CurrentPageChanged -= OnTabbedPageCurrentPageChanged;
            RemoveBinding(BindingContextProperty);
        }

        private void OnTabbedPageCurrentPageChanged(object sender, EventArgs e)
        {
            SelectedItem = page.CurrentPage?.BindingContext;
        }

        private void PerformTabBinding()
        {
            if(page == null)
            {
                return;
            }

            page.Children.Clear();
            if(ItemsSource != null)
            {
                IDataTemplateSelector selector = ItemTemplateSelector;
                foreach (object item in ItemsSource)
                {
                    Page pg = CreateTabItemPage(item, selector?.SelectTemplate(item, page));
                    page.Children.Add(pg);
                }
            }
        }

        private void InstallItemsSourceChangeEvent(INotifyCollectionChanged items, bool install)
        {
            if(items != null)
            {
                if(install)
                {
                    items.CollectionChanged += OnItemsSourceCollectionChanged;
                }
                else
                {
                    items.CollectionChanged -= OnItemsSourceCollectionChanged;
                }
            }
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(page == null)
            {
                return;
            }
            IDataTemplateSelector selector = ItemTemplateSelector;
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                page.Children.Clear();
            }
            else if(e.Action == NotifyCollectionChangedAction.Add)
            {
                int idx = e.NewStartingIndex;
                foreach(object item in e.NewItems)
                {
                    page.Children.Insert(idx++, CreateTabItemPage(item, selector?.SelectTemplate(item, page)));
                }
            }
            else if(e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach(object item in e.OldItems)
                {
                    page.Children.RemoveAt(e.OldStartingIndex);
                }
            }
            else if(e.Action == NotifyCollectionChangedAction.Replace)
            {
                int idx = e.NewStartingIndex;
                foreach (object item in e.NewItems)
                {
                    page.Children[idx++] = CreateTabItemPage(item, selector?.SelectTemplate(item, page));
                }
            }
            else
            {
                throw new NotSupportedException(e.Action.ToString());
            }
        }


        private Page CreateTabItemPage(object item, DataTemplate itemTemplate)
        {
            Page pg;
            if (itemTemplate == null)
            {
                pg = new ContentPage
                {
                    Content = new Label() { Text = item.ToString() }
                };
            }
            else
            {
                object content = itemTemplate.CreateContent();
                pg = content as Page;
                if (pg == null)
                {
                    pg = new ContentPage
                    {
                        Content = content as View ?? new Label() { Text = item.ToString() }
                    };
                }
            }
            pg.BindingContext = item;
            return pg;

        }

    }
}
