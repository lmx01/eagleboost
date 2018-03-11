namespace eagleboost.shell.ShellTreeView.Contracts
{
  using System.Collections.Generic;
  using eagleboost.shell.ShellTreeView.Data;
  using Microsoft.WindowsAPICodePack.Shell;

  public interface IShellItemsOperation
  {
    #region Methods
    IEnumerable<IShellTreeItem> CreateChildItems(IEnumerable<ShellObject> shellObjects, ShellTreeContainerItem parent);

    bool Filter(IShellTreeItem item);
    #endregion Methods    
  }
}