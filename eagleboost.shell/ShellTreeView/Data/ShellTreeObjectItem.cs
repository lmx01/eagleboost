namespace eagleboost.shell.ShellTreeView.Data
{
  using System.Diagnostics;
  using eagleboost.shell.ShellTreeView.Contracts;
  using Microsoft.WindowsAPICodePack.Shell;

  [DebuggerDisplay("{Name}")]
  public class ShellTreeObjectItem : ShellTreeItem
  {
    public ShellTreeObjectItem(ShellObject shellObj, ShellTreeContainerItem parent, IShellItemsOperation shellItemsOperation) : base(shellObj, parent, shellItemsOperation)
    {
    }
  }
}