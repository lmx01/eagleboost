// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 22 9:53 PM

namespace eagleboost.googledrive.Views
{
  using eagleboost.googledrive.Contracts;
  using eagleboost.shell.FileSystems.Models;

  /// <summary>
  /// GoogleDrivePopupTreeParams
  /// </summary>
  public class GoogleDrivePopupTreeParams
  {
    public IGoogleDriveService DriveService { get; set; }

    public string Path { get; set; }

    public FileFrequency[] FrequentFiles { get; set; }
  }
}