// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-26 6:42 PM

namespace eagleboost.googledrive.ViewModels
{
  using eagleboost.googledrive.Contracts;
  using eagleboost.shell.FileSystems.Contracts;
  using eagleboost.shell.FileSystems.ViewModels;

  /// <summary>
  /// IGoogleDriveFolderPathViewModel
  /// </summary>
  public interface IGoogleDriveFolderPathViewModel : IFileSystemFolderPathViewModel<IGoogleDriveFile, IGoogleDriveFolder>
  {
  }

  /// <summary>
  /// GoogleDriveFolderPathViewModel
  /// </summary>
  public class GoogleDriveFolderPathViewModel : FileSystemFolderPathViewModel<IGoogleDriveFile, IGoogleDriveFolder>, IGoogleDriveFolderPathViewModel
  {
  }
}