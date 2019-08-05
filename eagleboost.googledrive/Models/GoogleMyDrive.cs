// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-30 3:51 PM

namespace eagleboost.googledrive.Models
{
  using eagleboost.googledrive.Contracts;
  using Google.Apis.Drive.v3.Data;

  public sealed class GoogleMyDrive : File, IGoogleRootFile
  {
    #region ctors
    public GoogleMyDrive(File file)
    {
      Id = file.Id;
      Name = file.Name;
      OwnedByMe = file.OwnedByMe;
      WebViewLink = file.WebViewLink;
    }
    #endregion ctors 
  }
}