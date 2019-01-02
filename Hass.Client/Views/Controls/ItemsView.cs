using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace Hass.Client.Views.Controls
{

    public class ItemsView : ContentView
    {

        class DefaultTemplateSelector: IDataTemplateSelector
        {
            private ItemsView owner;

            public DefaultTemplateSelector(ItemsView owner)
            {
                this.owner = owner;
            }

            public DataTemplate SelectTemplate(object item, BindableObject container)
            {
                string key = item.GetType().FullName;
                object resx;
                if (owner.Resources.TryGetValue(key, out resx))
                {
                    return resx as DataTemplate;
                }

                resx = FindResource(owner.Parent as VisualElement, key);
                owner.Resources[key] = resx;

                return FindResource(owner, item.GetType().FullName) as DataTemplate;
            }

            private object FindResource(VisualElement el, string resxKey)
            {
                if(el == null)
                {
                    return null;
                }
                object resx;
                if (el.Resources.TryGetValue(resxKey, out resx))
                {
                    if(el != owner)
                    {
                        owner.Resources[resxKey] = resx;
                    }
                    return resx;
                }
                return FindResource(el.Parent as VisualElement, resxKey);
            }
        }


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
            propertyChanged: (bindable, oldvalue, newvalue) => ((ItemsView)bindable).RebuildChildren());

        public static readonly BindableProperty ItemTemplateSelectorProperty = BindableProperty.Create(
            "ItemTemplateSelector",
            typeof(IDataTemplateSelector),
            typeof(ItemsView),
            propertyChanged: (bindable, oldvalue, newvalue) => ((ItemsView)bindable).RebuildChildren());


        public static readonly BindableProperty ItemContainerStyleProperty = BindableProperty.Create(
            "ItemContainerStyle",
            typeof(Style),
            typeof(ItemsView),
            propertyChanged: (bindable, oldvalue, newvalue) => ((ItemsView)bindable).OnItemContainerStyleChanged());


        private TapGestureRecognizer tabGestureRecognizer;

        public ItemsView()
        {
            Layout = new StackLayout();
            tabGestureRecognizer = new TapGestureRecognizer();
            tabGestureRecognizer.Tapped += OnTapped;
        }

        private void OnTapped(object sender, EventArgs e)
        {
            var itemCtrl = sender as ItemContainer;
            if(itemCtrl != null)
            {
                itemCtrl.IsSelected = true;
            }
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

        public IDataTemplateSelector ItemTemplateSelector
        {
            get { return (IDataTemplateSelector)GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }

        private void OnLayoutChanged(Layout<View> oldValue, Layout<View> newValue)
        {
            Content = null;
            if (oldValue != null)
            {
                oldValue.Children.Clear();
            }
            Content = newValue;
            RebuildChildren();
        }

        private void OnItemContainerStyleChanged()
        {
            IList<View> children = Layout?.Children;
            if(children != null)
            {
                Style style = ItemContainerStyle;
                foreach(ItemContainer cnt in children)
                {
                    cnt.Style = style;
                }
            }
        }

        private void RebuildChildren()
        {
            if (Layout != null)
            {
                Layout.Children.Clear();
                BuildChildren(ItemsSource, 0);
            }
        }

        private void BuildChildren(IEnumerable items, int startingIndex)
        {
            IList<View> children = Layout?.Children;


            if (children == null || items == null)
            {
                return;
            }


            IDataTemplateSelector templateSel = ItemTemplateSelector;
            if(templateSel == null && ItemTemplate == null)
            {
                templateSel = new DefaultTemplateSelector(this);
            }

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

        private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            Action<INotifyCollectionChanged, bool> installEvtHandler = (col, install) =>
            {
                if (col != null)
                {
                    if (install)
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
            if (children == null)
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

        private ItemContainer currentSelected;

        public void ProcessItemIsSelectedChanged(ItemContainer itemContainer)
        {
            if(itemContainer.IsSelected && itemContainer != currentSelected)
            {
                if(currentSelected?.IsSelected == true)
                {
                    currentSelected.IsSelected = false;
                }
                currentSelected = itemContainer;
            }
        }

        private View CreateItemView(object item, DataTemplate itemTemplate)
        {
            ItemContainer itemView = new ItemContainer(this) {
                BindingContext = item,
                Style = ItemContainerStyle,
            };

            itemView.GestureRecognizers.Add(tabGestureRecognizer);

            if (itemTemplate == null)
            {
                itemView.Content = new Label() { Text = item.ToString() };
            }
            else
            {
                View v = (View)itemTemplate.CreateContent();
                //FlexBasis basis = FlexLayout.GetBasis(v);
                //if (basis.Length > 0)
                //{
                //    FlexLayout.SetBasis(itemView, basis);
                //}
                //FlexAlignSelf alignSelf = FlexLayout.GetAlignSelf(v);
                //if (alignSelf != FlexAlignSelf.Auto)
                //{
                //    FlexLayout.SetAlignSelf(itemView, alignSelf);
                //}
                //float grow = FlexLayout.GetGrow(v);
                //if (grow > 0)
                //{
                //    FlexLayout.SetGrow(itemView, grow);
                //}
                //float shrink = FlexLayout.GetShrink(v);
                //if (shrink > 0)
                //{
                //    FlexLayout.SetShrink(itemView, shrink);
                //}
                //int order = FlexLayout.GetOrder(v);
                //if (order > 0)
                //{
                //    FlexLayout.SetOrder(itemView, order);
                //}
                var wrapLayout = Layout as GridWrapLayout;
                if(wrapLayout != null)
                {
                    int colSpan = GridWrapLayout.GetColumnSpan(v);
                    if (colSpan > 1)
                    {
                        GridWrapLayout.SetColumnSpan(itemView, colSpan);
                    }
                    int rowSpan = GridWrapLayout.GetRowSpan(v);
                    if (rowSpan > 1)
                    {
                        GridWrapLayout.SetRowSpan(itemView, rowSpan);
                    }
                }
                itemView.Content = v;
            }
            return itemView;
        }

    }
}