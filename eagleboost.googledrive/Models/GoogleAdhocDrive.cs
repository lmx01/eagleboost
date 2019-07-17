// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 09 10:44 PM

namespace eagleboost.googledrive.Models
{
  using eagleboost.googledrive.Contracts;
  using Google.Apis.Drive.v3.Data;

  /// <summary>
  /// GoogleAdhocDrive
  /// </summary>
  public sealed class GoogleAdhocDrive : File, IGoogleRootFile
  {
    #region ctors
    public GoogleAdhocDrive()
    {
      Id = "$adhoc$";
      Name = "Adhoc";
      OwnedByMe = true;
    }
    #endregion ctors 
  }
}