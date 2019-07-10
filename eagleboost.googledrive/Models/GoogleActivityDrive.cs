// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 09 8:52 PM

namespace eagleboost.googledrive.Models
{
  using Google.Apis.Drive.v3.Data;

  /// <summary>
  /// GoogleActivityDrive
  /// </summary>
  public sealed class GoogleActivityDrive : File
  {
    #region ctors
    public GoogleActivityDrive()
    {
      Id = "$activity$";
      Name = "Activity";
      OwnedByMe = true;
    }
    #endregion ctors 
  }
}