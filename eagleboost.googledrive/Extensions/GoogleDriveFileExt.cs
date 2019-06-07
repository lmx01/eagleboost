﻿// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-21 3:55 PM

namespace eagleboost.googledrive.Extensions
{
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Types;

  public static class GoogleDriveFileExt
  {
    public static bool IsFolder(this IGoogleDriveFile file)
    {
      return file is IGoogleDriveFolder && file.Type == MimeType.Folder;
    }
  }
}