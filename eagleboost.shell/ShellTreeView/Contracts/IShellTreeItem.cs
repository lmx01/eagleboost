namespace eagleboost.shell.ShellTreeView.Contracts
{
  using System.Collections.Generic;
  using System.ComponentModel;
  using Microsoft.WindowsAPICodePack.Shell;

  public interface IShellTreeItem : INotifyPropertyChanged
  {
    #region Properties
    string Name { get; }
    
    ShellObject ShellObject { get; }
    
    IReadOnlyCollection<IShellTreeItem> Children { get; }
    
    bool IsExpanded { get; set; }
    
    bool IsSelected { get; set; }
    #endregion    
  }
}