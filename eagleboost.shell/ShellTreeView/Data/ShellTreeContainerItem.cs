namespace eagleboost.shell.ShellTreeView.Data
{
  using System.Diagnostics;
  using eagleboost.shell.ShellTreeView.Contracts;
  using Microsoft.WindowsAPICodePack.Shell;

  [DebuggerDisplay("{Name}")]
  public class ShellTreeContainerItem : ShellTreeItem
  {
    public ShellTreeContainerItem(ShellObject shellObj, ShellTreeContainerItem parent, IShellItemsOperation shellItemsOperation) : base(shellObj, parent, shellItemsOperation)
    {
    }
  }
}