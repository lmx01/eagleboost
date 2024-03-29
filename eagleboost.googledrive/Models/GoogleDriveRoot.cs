﻿// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 16 8:26 AM

namespace eagleboost.googledrive.Models
{
  using Google.Apis.Drive.v3.Data;

  public sealed class GoogleDriveRoot : File
  {
    #region ctors
    public GoogleDriveRoot()
    {
      Id = "$root$";
      Name = "Google Drive";
      OwnedByMe = true;
    }
    #endregion ctors 
  }
}