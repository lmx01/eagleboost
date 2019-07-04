// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 15 5:33 PM

namespace eagleboost.googledrive.Models
{
  using System.IO;
  using System.Windows.Media;
  using eagleboost.googledrive.Contracts;

  public class GoogleDriveImageFileInfoModel : GoogleDriveFileInfoModel
  {
    public GoogleDriveImageFileInfoModel(IGoogleDriveFile file) : base(file)
    {
    }

    #region Public Properties
    public ImageSource ImageSource { get; set; }

    public Stream ImageStream { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }
    #endregion Public Properties
  }
}