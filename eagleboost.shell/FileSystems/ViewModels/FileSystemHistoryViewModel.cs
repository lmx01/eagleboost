// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 23 8:33 PM

namespace eagleboost.shell.FileSystems.ViewModels
{
  using System;
  using System.ComponentModel;
  using System.Threading.Tasks;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.Data;
  using eagleboost.core.Extensions;
  using eagleboost.presentation.Collections;
  using eagleboost.presentation.Controls.TreeView;
  using eagleboost.shell.FileSystems.Contracts;
  using eagleboost.shell.FileSystems.Extensions;

  /// <summary>
  /// FileSystemHistoryViewModel
  /// </summary>
  /// <typeparam name="TFile"></typeparam>
  /// <typeparam name="TFolder"></typeparam>
  public class FileSystemHistoryViewModel<TFile, TFolder> : NotifyPropertyChangedBase, IFileSystemHistoryViewModel<TFile, TFolder>
    where TFile : class, IFile
    where TFolder : class, IFolder
  {
    #region Declarations
    private readonly FileSystemHistory _history;
    private IFileSystemTreeViewModel _treeViewModel;
    private IFileSystemCollectionViewModel<TFile, TFolder> _gridViewModel;
    private DisposeManager _gridSelectionToken;
    #endregion Declarations

    #region ctors
    public FileSystemHistoryViewModel()
    {
      _history = new FileSystemHistory();
    }
    #endregion ctors

    #region IFileSystemHistoryViewModel
    public FileSystemHistory History
    {
      get { return _history; }
    }

    public void Initialize(IFileSystemTreeViewModel treeViewModel, IFileSystemCollectionViewModel<TFile, TFolder> gridViewModel)
    {
      _treeViewModel = treeViewModel;
      _treeViewModel.PropertyChanged += HandleTreePropertyChanged;
      _gridViewModel = gridViewModel;
    }
    #endregion IFileSystemHistoryViewModel

    #region Event Handlers
    private void HandleTreePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.Match<ICollectionViewModel>(v => v.SelectedItem))
      {
        var selected = (TreeNodeContainer)_treeViewModel.SelectedItem;
        if (selected != null)
        {
          UpdateSelectedFolders((TFolder)selected.DataItem);
        }
        else
        {
          UpdateEmptySelectedFolders();
        }
      }
    }
    #endregion Event Handlers

    #region Private Methods
    private void UpdateSelectedFolders(TFolder folder)
    {
      var entry = new FileSystemHistoryOperations(folder, HandleNavigateEntry);
      History.Add(entry);
    }

    private void UpdateEmptySelectedFolders()
    {
      History.Clear();
    }

    private void HandleNavigateEntry(IFileSystemHistoryOperations entry)
    {
      NavigateAsync(entry.DriveFolder).ConfigureAwait(false);
    }

    private async Task NavigateAsync(IFolder folder)
    {
      _treeViewModel.PropertyChanged -= HandleTreePropertyChanged;

      if (_gridSelectionToken != null)
      {
        _gridSelectionToken.Dispose();
      }

      var gvm = _gridViewModel;

      EventHandler handler = (s, e) => gvm.SetSelectedAsync(folder as TFile);

      var token = _gridSelectionToken = new DisposeManager();
      token.AddEvent(h => gvm.FilesPopulated += h, h => gvm.FilesPopulated -= h, handler);

      await _treeViewModel.SelectAsync(folder);
      _treeViewModel.PropertyChanged += HandleTreePropertyChanged;
    }
    #endregion Private Methods
  }
}