// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-21 10:24 PM

namespace eagleboost.googledrive.Extensions
{
  using eagleboost.googledrive.Types;
  using Google.Apis.Drive.v3.Data;

  public static class FileExt
  {
    public static bool IsFolder(this File file)
    {
      return file.MimeType == MimeType.Folder;
    }
  }
}