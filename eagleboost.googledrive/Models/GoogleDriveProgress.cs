// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-04 11:51 PM

namespace eagleboost.googledrive.Models
{
  using eagleboost.googledrive.Contracts;

  /// <summary>
  /// GoogleDriveProgress
  /// </summary>
  public class GoogleDriveProgress
  {
    public int Count { get; set; }

    public IGoogleDriveFile Current { get; set; }

    public string Status { get; set; }
  }
}