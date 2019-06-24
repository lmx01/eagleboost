// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 23 8:43 PM

namespace eagleboost.shell.FileSystems.ViewModels
{
  using System;
  using System.Windows.Input;
  using eagleboost.core.ComponentModel;
  using eagleboost.shell.FileSystems.Contracts;
  using Prism.Commands;

  /// <summary>
  /// FileSystemHistoryOperations
  /// </summary>
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
      NavigateToCommand = new DelegateCommand<IFileSystemHistoryOperations>(HandleNavigateTo);
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
    #endregion IFileSystemHistoryOperations

    #region Private Methods
    private void HandleNavigateTo(IFileSystemHistoryOperations folder)
    {
      _navigateToAction(folder);
    }
    #endregion Private Methods
  }
}