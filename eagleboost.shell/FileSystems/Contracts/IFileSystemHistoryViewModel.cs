// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 23 2:39 PM

namespace eagleboost.shell.FileSystems.Contracts
{
  using eagleboost.shell.FileSystems.ViewModels;

  /// <summary>
  /// IFileSystemHistoryViewModel
  /// </summary>
  /// <typeparam name="TFile"></typeparam>
  /// <typeparam name="TFolder"></typeparam>
  public interface IFileSystemHistoryViewModel<TFile, TFolder>
    where TFile : class, IFile
    where TFolder : IFolder
  {
    #region Properties
    FileSystemHistory History { get; }
    #endregion Properties

    #region Methods
    void Initialize(IFileSystemTreeViewModel treeViewModel, IFileSystemCollectionViewModel<TFile, TFolder> gridViewModel);
    #endregion Methods
  }
}