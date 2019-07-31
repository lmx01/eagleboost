// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-01 12:11 PM

namespace eagleboost.shell.FileSystems.Contracts
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.presentation.Collections;
  using eagleboost.presentation.Controls.TreeView;

  /// <summary>
  /// IFileSystemCollectionViewModel
  /// </summary>
  public interface IFileSystemCollectionViewModel : ICollectionViewModel
  {
    #region Methods
    Task<IReadOnlyCollection<IFile>> SetFolderAsync(ITreeNodeContainer folderNode, CancellationToken ct = default(CancellationToken));

    Task<IReadOnlyCollection<IFile>> SetFilesAsync(ITreeNodeContainer folderNode, Func<CancellationToken, Task<IReadOnlyCollection<IFile>>> fileFunc, CancellationToken ct = default(CancellationToken));
    #endregion Methods
  }

  /// <summary>
  /// IFileSystemCollectionViewModel
  /// </summary>
  /// <typeparam name="TFile"></typeparam>
  /// <typeparam name="TFolder"></typeparam>
  public interface IFileSystemCollectionViewModel<TFile, TFolder> : ICollectionViewModel<TFile>, INotifyPropertyChanged
    where TFile : class, IFile
    where TFolder : IFolder
  {
    #region Properties
    ITreeNodeContainer CurrentFolderNode { get; }

    TFolder CurrentFolder { get; }
    #endregion Properties

    #region Methods
    Task<IReadOnlyCollection<TFile>> SetFolderAsync(ITreeNodeContainer folderNode, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    Task<IReadOnlyCollection<TFile>> SetFilesAsync(ITreeNodeContainer folderNode, Func<CancellationToken, Task<IReadOnlyCollection<IFile>>> fileFunc, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    Task<TFile> SetSelectedAsync(TFile file, CancellationToken ct = default(CancellationToken));
    #endregion Methods

    #region Events
    event FileSelectedEventHandler<TFile> FileSelected;

    event EventHandler FilesPopulated;
    #endregion Events
  }

  public class FileSelectedEventArgs<TFile> : EventArgs where TFile : class, IFile
  {
    public readonly TFile File;

    #region ctors
    public FileSelectedEventArgs(TFile file)
    {
      File = file;
    }
    #endregion ctors
  }

  public delegate void FileSelectedEventHandler<TFile>(object sender, FileSelectedEventArgs<TFile> args) where TFile : class, IFile;
}