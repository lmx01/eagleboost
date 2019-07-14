// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 14 2:54 PM

namespace eagleboost.googledrive.Models
{
  using Google.Apis.Drive.v3.Data;

  /// <summary>
  /// GoogleSearchResultDrive
  /// </summary>
  public sealed class GoogleSearchResultDrive : File
  {
    #region ctors
    public GoogleSearchResultDrive()
    {
      Id = "$searchresult$";
      Name = "Search Result";
      OwnedByMe = true;
    }
    #endregion ctors 
  }
}