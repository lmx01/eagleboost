// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-21 9:17 PM

namespace eagleboost.googledrive.Contracts
{
  using eagleboost.shell.FileSystems.Contracts;
  using Google.Apis.Drive.v3.Data;

  /// <summary>
  /// IGoogleDriveFolder
  /// </summary>
  public interface IGoogleDriveFolder : IFolder, IGoogleDriveFile
  {
    #region Properties
    File File { get; }
    #endregion Properties
  }
}