// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-27 11:47 PM

namespace eagleboost.presentation.Controls.TreeView
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Interactivity;
  using eagleboost.presentation.Extensions;

  /// <summary>
  /// BringSelectedTreeItemIntoView - When SelectedItemChanged, bring the associated TreeViewItem into view
  /// </summary>
  public class BringSelectedTreeItemIntoView : Behavior<TreeView>
  {
    #region Overrides
    protected override void OnAttached()
    {
      base.OnAttached();

      var tree = AssociatedObject;
      tree.SelectedItemChanged += HandleSelectedItemChanged;
    }

    protected override void OnDetaching()
    {
      var tree = AssociatedObject;
      tree.SelectedItemChanged -= HandleSelectedItemChanged;

      base.OnDetaching();
    }

    private void HandleSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      var tree = AssociatedObject;
      var dataItem = e.NewValue;
      if (tree.IsLoaded && dataItem != null)
      {
        var treeViewItem = (TreeViewItem) tree.ItemContainerGenerator.ContainerFromItem(dataItem);
        if (treeViewItem != null)
        {
          treeViewItem.Dispatcher.BeginInvoke(() => treeViewItem.BringIntoView());
        }
      }
    }
    #endregion Overrides
  }
}