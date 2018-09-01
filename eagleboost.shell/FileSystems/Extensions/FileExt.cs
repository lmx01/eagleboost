// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-30 9:32 PM

namespace eagleboost.shell.FileSystems.Extensions
{
  using System.Collections.Generic;
  using eagleboost.shell.FileSystems.Contracts;

  public static class FileExt
  {
    public static bool IsFolder(this IFile file)
    {
      return file is IFolder && file.Type == "Folder";
    }

    public static IEnumerable<TFolder> FolderToRoot<TFolder>(this IFile file) where TFolder : class, IFolder
    {
      if (file != null)
      {
        var folder = file as TFolder;
        var f = folder ?? file.Parent;
        while (f != null)
        {
          yield return (TFolder)f;
          f = f.Parent;
        }
      }
    }
  }
}