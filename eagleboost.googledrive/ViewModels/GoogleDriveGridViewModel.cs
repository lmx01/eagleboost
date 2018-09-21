// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-24 12:58 AM

namespace eagleboost.googledrive.ViewModels
{
  using eagleboost.googledrive.Contracts;
  using eagleboost.shell.FileSystems.Contracts;
  using eagleboost.shell.FileSystems.ViewModels;

  /// <summary>
  /// IGoogleDriveGridViewModel
  /// </summary>
  public interface IGoogleDriveGridViewModel : IFileSystemCollectionViewModel<IGoogleDriveFile, IGoogleDriveFolder>
  {
  }

  /// <summary>
  /// GoogleDriveGridViewModel
  /// </summary>
  public class GoogleDriveGridViewModel: FileSystemCollectionViewModel<IGoogleDriveFile, IGoogleDriveFolder>, IGoogleDriveGridViewModel
  {
  }
}