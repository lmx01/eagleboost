namespace eagleboost.shell.ShellTreeView.ViewModels
{
  using System.ComponentModel;
  using eagleboost.core.Extensions;

  public partial class ShellTreeViewModel
  {
    private void HandleOptionsChanged(object sender, PropertyChangedEventArgs e)
    {
      if (ShellTreeViewOptions.ShowFilesArgs.Match(e))
      {
        foreach (var expandedItem in _expandedItems)
        {
          expandedItem.Refresh();
        }
      }
    }
  }
}