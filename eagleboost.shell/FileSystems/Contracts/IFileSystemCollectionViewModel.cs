// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-01 12:11 PM

namespace eagleboost.shell.FileSystems.Contracts
{
  using System;
  using System.Collections.Generic;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.presentation.Collections;

  /// <summary>
  /// IFileSystemCollectionViewModel
  /// </summary>
  public interface IFileSystemCollectionViewModel : ICollectionViewModel
  {
    #region Methods
    Task<IReadOnlyList<IFile>> SetFolderAsync(IFolder folder, CancellationToken ct = default(CancellationToken));
    #endregion Methods
  }

  /// <summary>
  /// IFileSystemCollectionViewModel
  /// </summary>
  /// <typeparam name="TFile"></typeparam>
  /// <typeparam name="TFolder"></typeparam>
  public interface IFileSystemCollectionViewModel<TFile, TFolder> : ICollectionViewModel<TFile>
    where TFile : class, IFile
    where TFolder : IFolder
  {
    #region Properties

    TFolder CurrentFolder { get; }

    #endregion Properties

    #region Methods
    Task<IReadOnlyList<TFile>> SetFolderAsync(TFolder folder, CancellationToken ct = default(CancellationToken));
    #endregion Methods

    #region Events
    event FileSelectedEventHandler<TFile> FileSelected;
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