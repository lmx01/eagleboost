// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 16 8:23 AM

namespace eagleboost.googledrive.Models
{
  using eagleboost.googledrive.Contracts;
  using Google.Apis.Drive.v3.Data;

  public sealed class GoogleTeamDrive : File, IGoogleRootFile
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