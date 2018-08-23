// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-20 9:55 PM

namespace eagleboost.presentation.Controls.DataGrids
{
  using System;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Linq;
  using System.Windows.Controls;
  using System.Windows.Data;
  using System.Windows.Interactivity;
  using System.Windows.Markup;
  using eagleboost.core.Extensions;

  [ContentProperty("Items")]
  public class DataGridPrioritySorting : Behavior<DataGrid>
  {
    #region ctors
    public DataGridPrioritySorting()
    {
      Items = new Collection<DataGridPrioritySortingItem>();
    }
    #endregion ctors

    #region Public Properties
    public Collection<DataGridPrioritySortingItem> Items { get; set; }

    public DataGridPrioritySortingItem DefaultItem { get; set; }
    #endregion Public Properties

    #region Overrides

    protected override void OnAttached()
    {
      base.OnAttached();

      var grid = AssociatedObject;
      grid.Sorting += HandleGridSorting;
    }

    protected override void OnDetaching()
    {
      var grid = AssociatedObject;
      if (grid != null)
      {
        grid.Sorting -= HandleGridSorting;
      }

      base.OnDetaching();
    }
    #endregion Overrides

    #region Event Handlers
    private void HandleGridSorting(object sender, DataGridSortingEventArgs e)
    {
      var column = e.Column;
      if (column == null)
      {
        return;
      }

      var columnName = column.SortMemberPath;
      var sortingItem = Items.FirstOrDefault(i => i.Column == columnName) ?? DefaultItem;
      if (sortingItem == null || sortingItem.Comparer == null)
      {
        return;
      }

      if (!sortingItem.Comparer.IsCompatiableWith<IColumnComparer>())
      {
        throw new ApplicationException(string.Format("{0} is not IColumnComparer", sortingItem.Comparer));
      }

      // prevent the built-in sort from sorting
      e.Handled = true;

      var direction = column.SortDirection != ListSortDirection.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending;

      //set the sort order on the column
      column.SortDirection = direction;

      var grid = (DataGrid) sender;
      var view = grid.ItemsSource as ListCollectionView ?? (ListCollectionView)CollectionViewSource.GetDefaultView(grid.ItemsSource);

      var comparer = (IColumnComparer)Activator.CreateInstance(sortingItem.Comparer);
      comparer.Column = columnName;
      comparer.Direction = direction;

      //apply the sort
      view.CustomSort = comparer;
    }
    #endregion Event Handlers
  }
}