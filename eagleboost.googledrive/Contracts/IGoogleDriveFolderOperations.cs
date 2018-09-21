// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-26 6:29 PM

namespace eagleboost.googledrive.Contracts
{
  using System.Windows.Input;
  using eagleboost.core.ComponentModel;

  /// <summary>
  /// IGoogleDriveFolderOperations
  /// </summary>
  public interface IGoogleDriveFolderOperations : IDisplayItem
  {
    #region Properties
    IGoogleDriveFolder DriveFolder { get; }

    ICommand NavigateToCommand { get; }
    #endregion Properties
  }
}