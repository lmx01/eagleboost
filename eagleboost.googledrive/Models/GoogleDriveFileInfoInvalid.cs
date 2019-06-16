// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 15 6:52 PM

namespace eagleboost.googledrive.Models
{
  using eagleboost.googledrive.Contracts;

  public class GoogleDriveFileInfoInvalid : GoogleDriveFileInfoModel
  {
    public GoogleDriveFileInfoInvalid(IGoogleDriveFile file) : base(file)
    {
    }
  }
}