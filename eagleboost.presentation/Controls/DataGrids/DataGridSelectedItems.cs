// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-24 9:30 AM

namespace eagleboost.presentation.Controls.DataGrids
{
  using System.Linq;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Interactivity;
  using eagleboost.core.Collections;
  using eagleboost.presentation.Extensions;
  using eagleboost.presentation.Utils;

  /// <summary>
  /// DataGridSelectedItems
  /// </summary>
  public class DataGridSelectedItems : Behavior<DataGrid>
  {
    #region Dependency Properties
    public static readonly DependencyProperty SelectionContainerProperty = DependencyProperty.Register(
      "SelectionContainer", typeof(ISelectionContainer), typeof(DataGridSelectedItems), new PropertyMetadata(OnSelectionContainerChanged));

    public ISelectionContainer SelectionContainer
    {
      get { return (ISelectionContainer) GetValue(SelectionContainerProperty); }
      set { SetValue(SelectionContainerProperty, value); }
    }

    private static void OnSelectionContainerChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((DataGridSelectedItems)obj).OnSelectionContainerChanged((ISelectionContainer)e.OldValue, (ISelectionContainer)e.NewValue);
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
      SelectionContainer = null;

      base.OnDetaching();
    }
    #endregion Overrides

    #region Private Methods
    private void OnSelectionContainerChanged(ISelectionContainer oldValue, ISelectionContainer newValue)
    {
      if (oldValue != null)
      {
        oldValue.ItemsSelected -= HandleItemsSelected;
      }

      if (newValue != null)
      {
        newValue.ItemsSelected += HandleItemsSelected;
      }
    }
    #endregion Private Methods

    #region Event Handlers
    private void HandleGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var grid = AssociatedObject;
      var container = SelectionContainer;
      if (container != null && grid !=null)
      {
        try
        {
          grid.SelectionChanged -= HandleGridSelectionChanged;
          container.Unselect(e.RemovedItems);
          container.Select(e.AddedItems);
        }
        finally
        {
          grid.SelectionChanged += HandleGridSelectionChanged;
        }
      }
    }

    private void HandleItemsSelected(object sender, ItemsSelectedEventArgs e)
    {
      var container = (ISelectionContainer)sender;
      var grid = AssociatedObject;
      if (grid != null)
      {
        grid.SelectionChanged -= HandleGridSelectionChanged;
        try
        {
          grid.SelectManyItems(container.SelectedItems);
          var first = container.SelectedItems.Cast<object>().First();
          grid.Dispatcher.BeginInvoke(() => grid.ScrollIntoView(first));
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