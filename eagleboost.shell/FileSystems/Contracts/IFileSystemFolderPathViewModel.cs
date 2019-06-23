// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 23 1:38 PM

namespace eagleboost.shell.FileSystems.Contracts
{
  using System.Collections.Generic;

  /// <summary>
  /// IFileSystemFolderPathViewModel
  /// </summary>
  public interface IFileSystemFolderPathViewModel<TFile, TFolder>
    where TFile : class, IFile
    where TFolder : IFolder
  {
    #region Properties
    IReadOnlyList<IFileSystemFolderOperations> SelectedFolders { get; }
    #endregion Properties

    #region Methods
    void Initialize(IFileSystemTreeViewModel treeViewModel, IFileSystemCollectionViewModel<TFile, TFolder> gridViewModel);
    #endregion Methods
  }
}