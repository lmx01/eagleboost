// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-25 3:12 AM

namespace eagleboost.googledrive.ViewModels
{
  using System;
  using System.Windows.Input;
  using eagleboost.core.ComponentModel;
  using eagleboost.googledrive.Contracts;
  using Prism.Commands;

  /// <summary>
  /// GoogleDriveFolderOperations
  /// </summary>
  public class GoogleDriveFolderOperations : NotifyPropertyChangedBase, IGoogleDriveFolderOperations
  {
    #region Declarations
    private readonly IGoogleDriveFolder _folder;
    private readonly Action<IGoogleDriveFolderOperations> _navigateToAction;
    #endregion Declarations

    #region ctors
    public GoogleDriveFolderOperations(IGoogleDriveFolder folder, Action<IGoogleDriveFolderOperations> navigateToAction)
    {
      _folder = folder;
      _navigateToAction = navigateToAction;
      NavigateToCommand = new DelegateCommand<IGoogleDriveFolderOperations>(HandleNavigateTo);
    }
    #endregion ctors

    #region IGoogleDriveFolderOperations
    public string Id
    {
      get { return _folder.Id; }
    }

    public string DisplayName
    {
      get { return _folder.DisplayName; }
    }

    public IGoogleDriveFolder DriveFolder
    {
      get { return _folder; }
    }

    public ICommand NavigateToCommand { get; private set; }
    #endregion IGoogleDriveFolderOperations

    #region Private Methods
    private void HandleNavigateTo(IGoogleDriveFolderOperations folder)
    {
      _navigateToAction(folder);
    }
    #endregion Private Methods
  }
}