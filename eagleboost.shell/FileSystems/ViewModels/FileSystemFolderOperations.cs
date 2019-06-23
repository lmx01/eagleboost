// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 23 2:15 PM

namespace eagleboost.shell.FileSystems.ViewModels
{
  using System;
  using System.Windows.Input;
  using eagleboost.core.ComponentModel;
  using eagleboost.shell.FileSystems.Contracts;
  using Prism.Commands;

  /// <summary>
  /// FileSystemFolderOperations
  /// </summary>
  public class FileSystemFolderOperations : NotifyPropertyChangedBase, IFileSystemFolderOperations
  {
    #region Declarations
    private readonly IFolder _folder;
    private readonly Action<IFileSystemFolderOperations> _navigateToAction;
    #endregion Declarations

    #region ctors
    public FileSystemFolderOperations(IFolder folder, Action<IFileSystemFolderOperations> navigateToAction)
    {
      _folder = folder;
      _navigateToAction = navigateToAction;
      NavigateToCommand = new DelegateCommand<IFileSystemFolderOperations>(HandleNavigateTo);
    }
    #endregion ctors

    #region IFileSystemFolderOperations
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
    #endregion IFileSystemFolderOperations

    #region Private Methods
    private void HandleNavigateTo(IFileSystemFolderOperations folder)
    {
      _navigateToAction(folder);
    }
    #endregion Private Methods
  }
}