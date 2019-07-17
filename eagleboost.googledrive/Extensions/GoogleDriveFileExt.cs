// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-21 3:55 PM

namespace eagleboost.googledrive.Extensions
{
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Models;
  using eagleboost.googledrive.Types;

  public static class GoogleDriveFileExt
  {
    public static bool IsFolder(this IGoogleDriveFile file)
    {
      return file is IGoogleDriveFolder && file.Type == MimeType.Folder;
    }

    public static IGoogleDriveFolder RootFolder(this IGoogleDriveFile file)
    {
      if (file == null)
      {
        return null;
      }

      var folder = file as IGoogleDriveFolder ?? (IGoogleDriveFolder)file.Parent;
      if (folder != null)
      {
        return GetRootFolder(folder);
      }

      return null;
    }

    private static IGoogleDriveFolder GetRootFolder(this IGoogleDriveFolder folder)
    {
      var p = folder;
      while (p != null)
      {
        if (p.File is IGoogleRootFile)
        {
          return p;
        }

        p = (IGoogleDriveFolder) p.Parent;
      }

      return null;
    }

    public static bool IsMyDriveFile(this IGoogleDriveFile file)
    {
      var root = file.RootFolder();
      return root != null && root.File is GoogleMyDrive;
    }

    public static bool IsTeamDriveFile(this IGoogleDriveFile file)
    {
      var root = file.RootFolder();
      return root != null && root.File is GoogleTeamDrive;
    }
  }
}