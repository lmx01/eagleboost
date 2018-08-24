// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-24 9:30 AM

namespace eagleboost.presentation.Controls.DataGrids
{
  using System;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Interactivity;
  using eagleboost.presentation.Contracts;
  using eagleboost.presentation.Utils;

  /// <summary>
  /// DataGridSelectedItems
  /// </summary>
  public class DataGridSelectedItems : Behavior<DataGrid>
  {
    #region Dependency Properties
    public static readonly DependencyProperty SelectedItemsSupportProperty = DependencyProperty.Register(
      "SelectedItemsSupport", typeof(ISelectedItemsSupport), typeof(DataGridSelectedItems), new PropertyMetadata(OnSelectedItemsSupportChanged));

    public ISelectedItemsSupport SelectedItemsSupport
    {
      get { return (ISelectedItemsSupport) GetValue(SelectedItemsSupportProperty); }
      set { SetValue(SelectedItemsSupportProperty, value); }
    }

    private static void OnSelectedItemsSupportChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((DataGridSelectedItems)obj).OnSelectedItemsSupportChanged((ISelectedItemsSupport)e.OldValue, (ISelectedItemsSupport)e.NewValue);
    }
    #endregion Dependency Properties 

    #region Overrides

    protected override void OnAttached()
    {
      base.OnAttached();

      var grid = AssociatedObject;
      grid.SelectionChanged += HandleGridSelectionChanged;
    }

    protected override void OnDetaching()
    {
      var grid = AssociatedObject;
      grid.SelectionChanged -= HandleGridSelectionChanged;
      SelectedItemsSupport = null;

      base.OnDetaching();
    }
    #endregion Overrides

    #region Private Methods
    private void OnSelectedItemsSupportChanged(ISelectedItemsSupport oldValue, ISelectedItemsSupport newValue)
    {
      if (oldValue != null)
      {
        oldValue.SelectedItemsChanged -= HandleViewModelSelectedItemsChanged;
      }

      if (newValue != null)
      {
        newValue.SelectedItemsChanged += HandleViewModelSelectedItemsChanged;
      }
    }
    #endregion Private Methods

    #region Event Handlers
    private void HandleGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var grid = (DataGrid)sender;
      var support = SelectedItemsSupport;
      support.SelectedItems = grid.SelectedItems;
    }

    private void HandleViewModelSelectedItemsChanged(object sender, EventArgs e)
    {
      var support = (ISelectedItemsSupport)sender;
      var grid = AssociatedObject;
      if (grid != null)
      {
        grid.SelectionChanged -= HandleGridSelectionChanged;
        try
        {
          grid.SelectManyItems(support.SelectedItems);
        }
        finally
        {
          grid.SelectionChanged -= HandleGridSelectionChanged;
        }
      }
    }
    #endregion Event Handlers
  }
}