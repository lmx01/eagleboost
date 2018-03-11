using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Shell;

namespace ShellHierarchyTreeDemo
{
  [DebuggerDisplay("{Name}")]
  public class ShellTreeObjectItem : ShellTreeItem
  {
    public ShellTreeObjectItem(ShellObject shellObj, ShellTreeContainerItem parent, IShellItemsOperation shellItemsOperation) : base(shellObj, parent, shellItemsOperation)
    {
    }
  }
}