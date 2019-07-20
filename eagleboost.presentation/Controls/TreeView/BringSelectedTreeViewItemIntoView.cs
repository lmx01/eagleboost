// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 14 9:18 PM

namespace eagleboost.presentation.Controls.TreeView
{
  using System.Windows;
  using System.Windows.Controls;

  /// <summary>
  /// BringSelectedTreeViewItemIntoView
  /// </summary>
  public class BringSelectedTreeViewItemIntoView
  {
    public static readonly DependencyProperty IsEnabledProperty =DependencyProperty.RegisterAttached(
        "IsEnabled",typeof(bool),typeof(BringSelectedTreeViewItemIntoView),new UIPropertyMetadata(false, OnIsEnabledChanged));

    public static bool GetIsEnabled(TreeViewItem treeViewItem)
    {
      return (bool)treeViewItem.GetValue(IsEnabledProperty);
    }

    public static void SetIsEnabled(TreeViewItem treeViewItem, bool value)
    {
      treeViewItem.SetValue(IsEnabledProperty, value);
    }

    private static void OnIsEnabledChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
    {
      var item = depObj as TreeViewItem;
      if (item == null)
      {
        return;
      }

      if (!(bool)e.NewValue)
      {
        item.Selected -= HandleTreeViewItemSelected;
        item.IsVisibleChanged -= HandleTreeViewItemIsVisibleChanged;
      }

      if ((bool) e.NewValue)
      {
        item.Selected += HandleTreeViewItemSelected;
        item.IsVisibleChanged += HandleTreeViewItemIsVisibleChanged;
      }
    }

    private static void HandleTreeViewItemSelected(object sender, RoutedEventArgs e)
    {
      if (!ReferenceEquals(sender, e.OriginalSource))
      {
        return;
      }

      var item = e.OriginalSource as TreeViewItem;
      if (item != null)
      {
        item.BringIntoView();
      }
    }

    private static void HandleTreeViewItemIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      if ((bool) e.NewValue)
      {
        var treeViewItem = (TreeViewItem)sender;
        if (treeViewItem.IsSelected)
        {
          treeViewItem.BringIntoView();
        }
      }
    }
  }
}