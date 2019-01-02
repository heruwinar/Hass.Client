using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Hass.Client.Views.Controls
{
    public class GridWrapLayout : Layout<View>
    {

        public static readonly BindableProperty CellMinWidthProperty = BindableProperty.Create(
            "CellMinWidth",
            typeof(double),
            typeof(GridWrapLayout),
            100.0,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                ((GridWrapLayout)bindable).InvalidateLayout();
            });

        public static readonly BindableProperty CellMinHeightProperty = BindableProperty.Create(
            "CellMinHeight",
            typeof(double),
            typeof(GridWrapLayout),
            80.0,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                ((GridWrapLayout)bindable).InvalidateLayout();
            });

        public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create(
            "ColumnSpacing",
            typeof(double),
            typeof(GridWrapLayout),
            5.0,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                ((GridWrapLayout)bindable).InvalidateLayout();
            });

        public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create(
            "RowSpacing",
            typeof(double),
            typeof(GridWrapLayout),
            5.0,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                ((GridWrapLayout)bindable).InvalidateLayout();
            });

        public static readonly BindableProperty ColumnSpanProperty = BindableProperty.CreateAttached(
            "ColumnSpan",
            typeof(int),
            typeof(GridWrapLayout),
            1,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                ((bindable as VisualElement)?.Parent as GridWrapLayout)?.InvalidateLayout();
            });

        public static readonly BindableProperty RowSpanProperty = BindableProperty.CreateAttached(
            "RowSpan",
            typeof(int),
            typeof(GridWrapLayout),
            1,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                ((bindable as VisualElement)?.Parent as GridWrapLayout)?.InvalidateLayout();
            });

        
        class LayoutItemData
        {
            private VisualElement element;
            double row;
            double column;
            int columnSpan;

            public LayoutItemData(VisualElement element, int row, int column, int columnSpan)
            {
                this.element = element;
                this.row = row;
                this.column = column;
                this.columnSpan = columnSpan;
            }


            public void PerformLayout(double cellWidth, double cellHeight, double rowSpacing, double columnSpacing)
            {
                int rowSpan = GetRowSpan(element);

                Size itemSize = new Size(rowSpan * cellWidth, columnSpan * cellHeight);

                LayoutChildIntoBoundingRegion(
                    element,
                    new Rectangle(
                        column * (cellWidth + columnSpacing),
                        row * (cellHeight + rowSpacing),
                        columnSpan * cellWidth + (columnSpan - 1) * columnSpacing,
                        rowSpan * cellHeight + (rowSpan - 1) * rowSpacing));
            }
        }

        class LayoutRow
        {
            private LayoutItemData[] cells;
            private int nextAvailableCell;
            private int columns;

            public LayoutRow(int rowNo, int columns)
            {
                RowNo = rowNo;
                this.columns = columns;
                cells = new LayoutItemData[columns];
            }

            public int RowNo { get; private set; }

            public int Columns { get { return columns; } }

            public LayoutItemData this[int column]
            {
                get
                {
                    return cells[column];
                }
            }

            public int NextAvailableColumn { get { return nextAvailableCell; } }

            public void Occupy(int column, int span, LayoutItemData item)
            {
                int maxC = column + span;
                for (int c = column; c < maxC; c++)
                {
                    cells[c] = item;
                }
                while (nextAvailableCell < columns && cells[nextAvailableCell] != null)
                {
                    nextAvailableCell++;
                }
            }

            public int? FindAvailableColumn(int startColumn, int columnSpan)
            {
                while (true)
                {
                    int upper = startColumn + columnSpan;
                    if (upper > columns)
                    {
                        return null;
                    }
                    int cell = startColumn;
                    for (; cell < upper; cell++)
                    {
                        if (cells[cell] != null)
                        {
                            while (cells[++cell] != null)
                            {
                                if (cell >= columns)
                                {
                                    return null;
                                }
                            }
                            startColumn = cell;
                            break;
                        }
                    }
                    if (cell == upper)
                    {
                        return startColumn;
                    }
                }
            }

            public bool DetermineCanOccupy(int column, int span)
            {
                int maxC = column + span;
                if (maxC >= columns)
                {
                    return false;
                }

                for (int c = column; c < maxC; c++)
                {
                    if (cells[c] != null)
                    {
                        return false;
                    }
                }
                return true;
            }

            public bool IsFull
            {
                get
                {
                    return nextAvailableCell >= columns;
                }
            }

            public IEnumerable<LayoutItemData> ListLayoutItems()
            {
                for(int i = 0; i < cells.Length; i ++)
                {
                    LayoutItemData item = cells[i];
                    if(item != null)
                    {
                        yield return item;
                    }
                }
            }
        }

        class LayoutTable
        {
            List<LayoutRow> rows;
            LayoutRow availableRow;
            Size cellSize;
            double columnSpacing;
            double rowSpacing;

            public LayoutTable(Size size, int columns, Size cellSize, double columnSpacing, double rowSpacing)
            {
                Size = size;
                Columns = columns;
                rows = new List<LayoutRow>(200);
                this.cellSize = cellSize;
                this.rowSpacing = rowSpacing;
                this.columnSpacing = columnSpacing;
            }

            public Size Size { get; private set; }
            public int Columns { get; private set; }

            public int Rows { get { return rows.Count; } }

           
            private LayoutRow GetRow(int row)
            {
                if (row >= rows.Count)
                {
                    rows.Add(new LayoutRow(row, Columns));
                }
                return rows[row];
            }

            public void LayoutItem(VisualElement visual)
            {
                int colSpan = Math.Min(Columns, GetColumnSpan(visual));
                int rowSpan = GetRowSpan(visual);

                if(availableRow == null)
                {
                    availableRow = GetRow(rows.Count);
                }

                LayoutRow startRow = availableRow;
                int startColumn = startRow.NextAvailableColumn;

                int? foundCol = null;

                int rowUpper = 0;
                while (foundCol == null)
                {
                    foundCol = startRow.FindAvailableColumn(startColumn, colSpan);
                    rowUpper = startRow.RowNo + rowSpan;
                    if (foundCol == null)
                    {
                        startRow = GetRow(startRow.RowNo + 1);
                        startColumn = startRow.NextAvailableColumn;
                        continue;
                    }
                    for (int r = startRow.RowNo + 1; r < rowUpper; r++)
                    {
                        LayoutRow nextRow = GetRow(r);
                        if (nextRow.IsFull)
                        {
                            foundCol = null;
                            startRow = GetRow(nextRow.RowNo + 1);
                            startColumn = startRow.NextAvailableColumn;
                            break;
                        }
                        if (GetRow(r).FindAvailableColumn(startColumn, colSpan) == null)
                        {
                            foundCol = null;
                            startColumn++;
                            break;
                        }
                    }
                }

                var item = new LayoutItemData(visual, startRow.RowNo, startColumn, colSpan);

                for (int r = startRow.RowNo; r < rowUpper; r++)
                {
                    rows[r].Occupy(startColumn, colSpan, item);
                }

                while (availableRow != null && availableRow.IsFull)
                {
                    int nextAvailableRowNo = availableRow.RowNo + 1;
                    availableRow = nextAvailableRowNo < rows.Count 
                        ? rows[nextAvailableRowNo] 
                        : null;
                }
            }

            public Size CalcTotalSize()
            {
                return new Size(
                    cellSize.Width * Columns + columnSpacing * (Columns - 1),
                    cellSize.Height * Rows + rowSpacing * (Rows - 1));
            }

            public void PerformLayout(double x, double y, double width, double height)
            {
                var visistedItems = new HashSet<LayoutItemData>();

                foreach (LayoutRow r in rows)
                {
                    foreach(LayoutItemData item in r.ListLayoutItems())
                    {
                        if (visistedItems.Contains(item))
                        {
                            continue;
                        }

                        visistedItems.Add(item);
                        item.PerformLayout(cellSize.Width, cellSize.Height, rowSpacing, columnSpacing);
                    }
                }
            }
        }

        private LayoutTable currentLayoutTable;

        public double CellMinHeight
        {
            get
            {
                return (double)GetValue(CellMinHeightProperty);
            }
            set
            {
                SetValue(CellMinHeightProperty, value);
            }
        }

        public double CellMinWidth
        {
            get
            {
                return (double)GetValue(CellMinWidthProperty);
            }
            set
            {
                SetValue(CellMinWidthProperty, value);
            }
        }

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

        public static int GetColumnSpan(VisualElement el)
        {
            return (int)el.GetValue(ColumnSpanProperty);
        }

        public static void SetColumnSpan(VisualElement el, int value)
        {
            el.SetValue(ColumnSpanProperty, value);
        }

        public static int GetRowSpan(VisualElement el)
        {
            return (int)el.GetValue(RowSpanProperty);
        }

        public static void SetRowSpan(VisualElement el, int value)
        {
            el.SetValue(RowSpanProperty, value);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            LayoutTable layoutData = GetLayoutData(widthConstraint, heightConstraint);

            if (layoutData == null || layoutData.Rows == 0)
            {
                return new SizeRequest();
            }

            return new SizeRequest(layoutData.CalcTotalSize());
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            GetLayoutData(width, height).PerformLayout(x, y, width, height);
        }

        private LayoutTable GetLayoutData(double width, double height)
        {
            Size size = new Size(width, height);

            // Check if cached information is available.
            if (currentLayoutTable != null && currentLayoutTable.Size == size)
            {
                return currentLayoutTable;
            }

            int columns = 0;
            Size maxChildSize = new Size(CellMinWidth, CellMinHeight);


            // Calculate the number of rows and columns.
            if (Double.IsPositiveInfinity(width))
            {
                columns = Children.Count(c => c.IsVisible);
            }
            else
            {
                columns = (int)((width + ColumnSpacing) / (maxChildSize.Width + ColumnSpacing));
                columns = Math.Max(1, columns);
            }

            // Now maximize the cell size based on the layout size.
            Size cellSize = new Size();

            cellSize.Width = Double.IsPositiveInfinity(width)
                ? maxChildSize.Width
                : (width - ColumnSpacing * (columns - 1)) / columns;

            cellSize.Height = maxChildSize.Height;

            var layoutTable = new LayoutTable(new Size(width, height), columns, cellSize, ColumnSpacing, RowSpacing);

            foreach(VisualElement el in Children)
            {
                if (el.IsVisible)
                {
                    layoutTable.LayoutItem(el);
                }
            }

            currentLayoutTable = layoutTable;

            return currentLayoutTable;
        }

        protected override void InvalidateLayout()
        {
            base.InvalidateLayout();

            // Discard all layout information for children added or removed.
            currentLayoutTable = null;
        }

        protected override void OnChildMeasureInvalidated()
        {
            base.OnChildMeasureInvalidated();

            // Discard all layout information for child size changed.
            currentLayoutTable = null;
        }

    }
}