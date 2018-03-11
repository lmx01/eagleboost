namespace eagleboost.presentation.Extensions
{
  using System.Windows.Controls;
  using System.Windows.Media;

  public static class TreeViewItemExt
  {
    public static int GetDepth(this TreeViewItem item)
    {
      TreeViewItem parent;
      while ((parent = GetParent(item)) != null)
      {
        return GetDepth(parent) + 1;
      }
      return 0;
    }

    private static TreeViewItem GetParent(TreeViewItem item)
    {
      var parent = VisualTreeHelper.GetParent(item);
      while (!(parent is TreeViewItem || parent is TreeView))
      {
        parent = VisualTreeHelper.GetParent(parent);
      }
      return parent as TreeViewItem;
    }
  }
}