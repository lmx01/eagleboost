// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-25 9:39 PM

namespace eagleboost.presentation.Controls.TreeView
{
  using System.Windows;
  using System.Windows.Controls;
  using eagleboost.presentation.Extensions;

  /// <summary>
  /// NonScrollTreeViewItem - When click on a TreeViewItem to select it, if horizontal scroll bar is visible, the TreeView would automatically scroll to the right to
  /// just show the left most of the selected item, we don't want that
  /// </summary>
  public static class NonScrollTreeViewItem
  {
    #region Attached Properties
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
      "IsEnabled", typeof(bool), typeof(NonScrollTreeViewItem), new PropertyMetadata(OnIsEnabledChanged));

    public static bool GetIsEnabled(DependencyObject obj)
    {
      return (bool)obj.GetValue(IsEnabledProperty);
    }

    public static void SetIsEnabled(DependencyObject obj, bool isEnabled)
    {
      obj.SetValue(IsEnabledProperty, isEnabled);
    }

    private static void OnIsEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      var treeViewItem = (TreeViewItem) obj;
      if (treeViewItem != null)
      {
        treeViewItem.RequestBringIntoView -= HandleRequestBringIntoView;
        if ((bool) e.NewValue)
        {
          treeViewItem.RequestBringIntoView += HandleRequestBringIntoView;
        }
      }
    }
    #endregion Attached Properties

    #region Event Handlers
    private static void HandleRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
    {
      var treeViewItem = (TreeViewItem)sender;
      if (treeViewItem.Parent == null)
      {
        var scrollViewer = treeViewItem.FindParent<ScrollViewer>();
        if (scrollViewer != null)
        {
          treeViewItem.Dispatcher.BeginInvoke(() => scrollViewer.ScrollToLeftEnd());
        }
      }
    }
    #endregion Event Handlers
  }
}