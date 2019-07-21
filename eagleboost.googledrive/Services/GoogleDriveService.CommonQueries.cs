// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-21 9:30 PM

namespace eagleboost.googledrive.Services
{
  using eagleboost.googledrive.Types;

  public partial class GoogleDriveService
  {
    public class CommonQueries
    {
      public static readonly string IsRoot = "'root' in parents";
      public static readonly string IsLive = "trashed = false";
      public static readonly string IsFolder = "mimeType = '" + MimeType.Folder + "'";
      public static readonly string IsNotFolder = "mimeType != '" + MimeType.Folder + "'";
      public static readonly string RootLiveFolder = IsRoot + " and " + IsLive + " and " + IsFolder;
      public static readonly string LiveFileFormat = "'{0}' in parents" + " and " + IsLive;
      public static readonly string FolderByNameFormat = "name = '{0}'" + " and " + IsLive + " and " + IsFolder;
    }
  }
}