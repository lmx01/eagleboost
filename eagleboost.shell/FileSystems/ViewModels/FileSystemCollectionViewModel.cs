// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-01 12:13 PM

namespace eagleboost.shell.FileSystems.ViewModels
{
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using System.Windows.Data;
  using eagleboost.core.Extensions;
  using eagleboost.presentation.Collections;
  using eagleboost.shell.FileSystems.Contracts;

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
    #endregion Declarations

    #region IFileSystemCollectionViewModel
    public TFolder CurrentFolder
    {
      get { return _currentFolder; }
      private set { SetValue(ref _currentFolder, value); }
    }

    async Task<IReadOnlyList<IFile>> IFileSystemCollectionViewModel.SetFolderAsync(IFolder folder, CancellationToken ct)
    {
      return await SetFolderAsync((TFolder) folder, ct).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<TFile>> SetFolderAsync(TFolder folder, CancellationToken ct = default(CancellationToken))
    {
      CurrentFolder = folder;

      Items.Clear();
      var files = await folder.GetFilesAsync(ct: ct).ConfigureAwait(true);
      var result = files.Cast<TFile>().ToArray();
      Items.AddRange(result);

      return result;
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
    #endregion IFileSystemCollectionViewModel

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