﻿// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-01 12:13 PM

namespace eagleboost.shell.FileSystems.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using System.Windows.Data;
  using eagleboost.core.Collections;
  using eagleboost.core.Extensions;
  using eagleboost.presentation.Collections;
  using eagleboost.presentation.Controls.TreeView;
  using eagleboost.shell.FileSystems.Contracts;
  using eagleboost.shell.FileSystems.Models;

  /// <summary>
  /// FileSystemCollectionViewModel
  /// </summary>
  /// <typeparam name="TFile"></typeparam>
  /// <typeparam name="TFolder"></typeparam>
  public class FileSystemCollectionViewModel<TFile, TFolder> : CollectionViewModelBase<TFile>, IFileSystemCollectionViewModel, IFileSystemCollectionViewModel<TFile, TFolder>
    where TFile : class, IFile
    where TFolder : IFolder
  {
    #region Statics
    protected static readonly string Name = Property<TFile>(o => o.Name);
    #endregion Statics

    #region Declarations
    private TFolder _currentFolder;
    private readonly Dictionary<string, FolderCacheEntry<TFile, TFolder>> _folderCache = new Dictionary<string, FolderCacheEntry<TFile, TFolder>>();
    #endregion Declarations

    #region IFileSystemCollectionViewModel
    public ITreeNodeContainer CurrentFolderNode { get; private set; }

    public TFolder CurrentFolder
    {
      get { return _currentFolder; }
      private set { SetValue(ref _currentFolder, value); }
    }

    async Task<IReadOnlyList<IFile>> IFileSystemCollectionViewModel.SetFolderAsync(ITreeNodeContainer folderNode, CancellationToken ct)
    {
      return await SetFolderAsync(folderNode, ct).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<TFile>> SetFolderAsync(ITreeNodeContainer folderNode, CancellationToken ct = default(CancellationToken))
    {
      CurrentFolderNode = folderNode;
      var folder = (TFolder) folderNode.DataItem;
      CurrentFolder = folder;

      Items.Clear();
      var result = await GetFilesAsync(folderNode, ct);
      Items.AddRange(result);

      RaiseFilesPopulated();

      return result;
    }

    public Task<TFile> SetSelectedAsync(TFile file, CancellationToken ct = default(CancellationToken))
    {
      if (file != null)
      {
        var items = Items;
        var item = items.FirstOrDefault(f => f.Id == file.Id);
        if (item != null)
        {
          SelectedItem = item;
          SelectionContainer.Select(item);
        }
        return Task.FromResult(item);
      }

      return Task.FromResult(default(TFile));
    }

    public event FileSelectedEventHandler<TFile> FileSelected;

    protected virtual void RaiseFileSelected(TFile file)
    {
      var handler = FileSelected;
      if (handler != null)
      {
        handler(this, new FileSelectedEventArgs<TFile>(file));
      }
    }

    public event EventHandler FilesPopulated;

    protected virtual void RaiseFilesPopulated()
    {
      var handler = FilesPopulated;
      if (handler != null)
      {
        handler(this, EventArgs.Empty);
      }
    }
    #endregion IFileSystemCollectionViewModel

    #region Virtuals
    protected virtual void OnFolderCacheLoaded(FolderCacheEntry<TFile, TFolder> cacheEntry)
    {
    }

    protected virtual FolderCacheEntry<TFile, TFolder> CreateFolderCache(TFolder folder, IReadOnlyList<TFile> files)
    {
      return new FolderCacheEntry<TFile, TFolder>(folder, files);
    }
    #endregion Virtuals

    #region Private Methods
    private async Task<IReadOnlyList<TFile>> GetFilesAsync(ITreeNodeContainer folderNode, CancellationToken ct = default(CancellationToken))
    {
      var folder = (TFolder)folderNode.DataItem;
      FolderCacheEntry<TFile, TFolder> cacheEntry;
      if (_folderCache.TryGetValue(folder.Id, out cacheEntry))
      {
        if (folderNode.Children.Count == cacheEntry.Files.Count)
        {
          OnFolderCacheLoaded(cacheEntry);
          if (!cacheEntry.NeedsRefresh)
          {
            return cacheEntry.Files;
          }
        }
      }

      var files = await folder.GetFilesAsync(ct: ct).ConfigureAwait(true);
      var result = files.Cast<TFile>().ToArray();
      _folderCache[folder.Id] = CreateFolderCache(folder, result);
      return result;
    }
    #endregion Private Methods

    #region Overrides
    protected override ICollectionView CreateItemsView()
    {
      var view = new ListCollectionView(Items);
      view.SortDescriptions.Add(new SortDescription(Name, ListSortDirection.Ascending));

      return view;
    }

    protected override void OnItemSelected(TFile item)
    {
      base.OnItemSelected(item);

      RaiseFileSelected(item);
    }
    #endregion Overrides
  }
}