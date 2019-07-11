// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 10 9:53 PM

namespace eagleboost.shell.FileSystems.Models
{
  using System.Collections.Generic;
  using eagleboost.shell.FileSystems.Contracts;

  /// <summary>
  /// FolderCacheEntry
  /// </summary>
  public class FolderCacheEntry<TFile, TFolder>
    where TFile : IFile
    where TFolder : IFolder
  {
    public FolderCacheEntry(TFolder folder, IReadOnlyList<TFile> files)
    {
      Folder = folder;
      Files = files;
    }

    public TFolder Folder { get; private set; }

    public IReadOnlyList<TFile> Files { get; private set; }

    public bool NeedsRefresh { get; set; }
  }
}