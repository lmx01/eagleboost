// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 16 8:23 AM

namespace eagleboost.googledrive.Models
{
  using Google.Apis.Drive.v3.Data;

  public sealed class GoogleTeamDrive : File
  {
    #region ctors
    public GoogleTeamDrive()
    {
      Id = "$teamdrive$";
      Name = "Team Drive";
      OwnedByMe = false;
    }
    #endregion ctors 
  }
}