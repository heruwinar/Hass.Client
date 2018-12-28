using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace Hass.Client.Views.Layouts
{
    public class WrapLayout : Layout<View>
    {
        Dictionary<Size, LayoutData> layoutDataCache = new Dictionary<Size, LayoutData>();

        public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create(
            "ColumnSpacing",
            typeof(double),
            typeof(WrapLayout),
            5.0,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                ((WrapLayout)bindable).InvalidateLayout();
            });

        public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create(
            "RowSpacing",
            typeof(double),
            typeof(WrapLayout),
            5.0,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                ((WrapLayout)bindable).InvalidateLayout();
            });


        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            "ItemsSource",
            typeof(IEnumerable),
            typeof(WrapLayout),
            null,
            propertyChanged: (bindable, oldvalue, newvalue) =>
                ((WrapLayout)bindable).OnItemsSourceChanged((IEnumerable)oldvalue, (IEnumerable)newvalue));

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
            "ItemTemplate",
            typeof(DataTemplate),
            typeof(WrapLayout),
            propertyChanged: (bindable, oldvalue, newvalue) =>((WrapLayout)bindable).RebuildChildren());

        public static readonly BindableProperty ItemTemplateSelectorProperty = BindableProperty.Create(
            "ItemTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(WrapLayout),
            propertyChanged: (bindable, oldvalue, newvalue) =>((WrapLayout)bindable).RebuildChildren());

        public double ColumnSpacing
        {
            set { SetValue(ColumnSpacingProperty, value); }
            get { return (double)GetValue(ColumnSpacingProperty); }
        }

        public double RowSpacing
        {
            set { SetValue(RowSpacingProperty, value); }
            get { return (double)GetValue(RowSpacingProperty); }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
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

        private void RebuildChildren()
        {
            Children.Clear();
            BuildChildren(ItemsSource, 0);
        }

        private void BuildChildren(IEnumerable items, int startingIndex)
        {
            if(items != null)
            {
                DataTemplateSelector templateSel = ItemTemplateSelector;
                if (templateSel != null)
                {
                    foreach (object item in ItemsSource)
                    {
                        Children.Insert(startingIndex++, CreateItemView(item, templateSel.SelectTemplate(item, this)));
                    }
                }
                else
                {
                    DataTemplate template = ItemTemplate;
                    foreach (object item in ItemsSource)
                    {
                        Children.Insert(startingIndex++, CreateItemView(item, template));
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

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    Children.Clear();
                    break;
                case NotifyCollectionChangedAction.Add:
                    BuildChildren(e.NewItems, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (object item in e.OldItems)
                    {
                        Children.RemoveAt(e.OldStartingIndex);
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

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            LayoutData layoutData = GetLayoutData(widthConstraint, heightConstraint);
            if (layoutData.VisibleChildCount == 0)
            {
                return new SizeRequest();
            }

            Size totalSize = new Size(layoutData.CellSize.Width * layoutData.Columns + ColumnSpacing * (layoutData.Columns - 1),
                                      layoutData.CellSize.Height * layoutData.Rows + RowSpacing * (layoutData.Rows - 1));
            return new SizeRequest(totalSize);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            LayoutData layoutData = GetLayoutData(width, height);

            if (layoutData.VisibleChildCount == 0)
            {
                return;
            }

            double xChild = x;
            double yChild = y;
            int row = 0;
            int column = 0;

            foreach (View child in Children)
            {
                if (!child.IsVisible)
                {
                    continue;
                }

                LayoutChildIntoBoundingRegion(child, new Rectangle(new Point(xChild, yChild), layoutData.CellSize));

                if (++column == layoutData.Columns)
                {
                    column = 0;
                    row++;
                    xChild = x;
                    yChild += RowSpacing + layoutData.CellSize.Height;
                }
                else
                {
                    xChild += ColumnSpacing + layoutData.CellSize.Width;
                }
            }
        }

        LayoutData GetLayoutData(double width, double height)
        {
            Size size = new Size(width, height);

            // Check if cached information is available.
            if (layoutDataCache.ContainsKey(size))
            {
                return layoutDataCache[size];
            }

            int visibleChildCount = 0;
            Size maxChildSize = new Size();
            int rows = 0;
            int columns = 0;
            LayoutData layoutData = new LayoutData();

            // Enumerate through all the children.
            foreach (View child in Children)
            {
                // Skip invisible children.
                if (!child.IsVisible)
                    continue;

                // Count the visible children.
                visibleChildCount++;

                // Get the child's requested size.
                SizeRequest childSizeRequest = child.Measure(Double.PositiveInfinity, Double.PositiveInfinity);

                // Accumulate the maximum child size.
                maxChildSize.Width = Math.Max(maxChildSize.Width, childSizeRequest.Request.Width);
                maxChildSize.Height = Math.Max(maxChildSize.Height, childSizeRequest.Request.Height);
            }

            if (visibleChildCount != 0)
            {
                // Calculate the number of rows and columns.
                if (Double.IsPositiveInfinity(width))
                {
                    columns = visibleChildCount;
                    rows = 1;
                }
                else
                {
                    columns = (int)((width + ColumnSpacing) / (maxChildSize.Width + ColumnSpacing));
                    columns = Math.Max(1, columns);
                    rows = (visibleChildCount + columns - 1) / columns;
                }

                // Now maximize the cell size based on the layout size.
                Size cellSize = new Size();

                if (Double.IsPositiveInfinity(width))
                {
                    cellSize.Width = maxChildSize.Width;
                }
                else
                {
                    cellSize.Width = (width - ColumnSpacing * (columns - 1)) / columns;
                }

                if (Double.IsPositiveInfinity(height))
                {
                    cellSize.Height = maxChildSize.Height;
                }
                else
                {
                    cellSize.Height = (height - RowSpacing * (rows - 1)) / rows;
                }

                layoutData = new LayoutData(visibleChildCount, cellSize, rows, columns);
            }

            layoutDataCache.Add(size, layoutData);
            return layoutData;
        }

        protected override void InvalidateLayout()
        {
            base.InvalidateLayout();

            // Discard all layout information for children added or removed.
            layoutDataCache.Clear();
        }

        protected override void OnChildMeasureInvalidated()
        {
            base.OnChildMeasureInvalidated();

            // Discard all layout information for child size changed.
            layoutDataCache.Clear();
        }
    }
}