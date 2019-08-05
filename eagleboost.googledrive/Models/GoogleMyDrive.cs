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
      foreach (var p in typeof(File).GetProperties())
      {
        p.SetValue(this, p.GetValue(file));
      }
    }
    #endregion ctors 
  }
}