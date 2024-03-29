﻿// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 23 8:43 PM

namespace eagleboost.shell.FileSystems.ViewModels
{
  using System;
  using System.Diagnostics;
  using System.Windows.Input;
  using eagleboost.core.ComponentModel;
  using eagleboost.shell.FileSystems.Contracts;
  using Prism.Commands;

  /// <summary>
  /// FileSystemHistoryOperations
  /// </summary>
  [DebuggerDisplay("{DriveFolder}")]
  public class FileSystemHistoryOperations : NotifyPropertyChangedBase, IFileSystemHistoryOperations
  {
    #region Declarations
    private readonly IFolder _folder;
    private readonly Action<IFileSystemHistoryOperations> _navigateToAction;
    #endregion Declarations

    #region ctors
    public FileSystemHistoryOperations(IFolder folder, Action<IFileSystemHistoryOperations> navigateToAction)
    {
      _folder = folder;
      _navigateToAction = navigateToAction;
      NavigateToCommand = new DelegateCommand(HandleNavigateTo);
    }
    #endregion ctors

    #region IFileSystemHistoryOperations
    public string Id
    {
      get { return _folder.Id; }
    }

    public string DisplayName
    {
      get { return _folder.DisplayName; }
    }

    public IFolder DriveFolder
    {
      get { return _folder; }
    }

    public ICommand NavigateToCommand { get; private set; }

    public void NavigateTo()
    {
      _navigateToAction(this);
    }

    public event EventHandler Navigate;

    internal void RaiseNavigate()
    {
      var handler = Navigate;
      if (handler != null)
      {
        handler(this, EventArgs.Empty);
      }
    }
    #endregion IFileSystemHistoryOperations

    #region Overrides

    public override string ToString()
    {
      return _folder.ToString();
    }

    #endregion Overrides

    #region Private Methods
    private void HandleNavigateTo()
    {
      NavigateTo();
      RaiseNavigate();
    }
    #endregion Private Methods
  }
}