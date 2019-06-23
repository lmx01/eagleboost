// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 23 1:36 PM

namespace eagleboost.shell.FileSystems.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.Data;
  using eagleboost.core.Extensions;
  using eagleboost.presentation.Collections;
  using eagleboost.presentation.Controls.TreeView;
  using eagleboost.shell.FileSystems.Contracts;
  using eagleboost.shell.FileSystems.Extensions;

  /// <summary>
  /// FileSystemFolderPathViewModel
  /// </summary>
  public class FileSystemFolderPathViewModel<TFile, TFolder> : NotifyPropertyChangedBase, IFileSystemFolderPathViewModel<TFile, TFolder>
    where TFile : class, IFile
    where TFolder : class, IFolder
  {
    #region Declarations
    private IFileSystemTreeViewModel _treeViewModel;
    private IFileSystemCollectionViewModel<TFile, TFolder> _gridViewModel;
    private IReadOnlyList<IFileSystemFolderOperations> _selectedFolders;
    private DisposeManager _gridSelectionToken;
    #endregion Declarations

    #region IFileSystemFolderPathViewModel
    public void Initialize(IFileSystemTreeViewModel treeViewModel, IFileSystemCollectionViewModel<TFile, TFolder> gridViewModel)
    {
      _treeViewModel = treeViewModel;
      _treeViewModel.PropertyChanged += HandleTreePropertyChanged;
      _gridViewModel = gridViewModel;
    }
    #endregion IFileSystemFolderPathViewModel

    #region IGoogleDriveFolderPathViewModel
    public IReadOnlyList<IFileSystemFolderOperations> SelectedFolders
    {
      get { return _selectedFolders; }
      private set { SetValue(ref _selectedFolders, value); }
    }
    #endregion IGoogleDriveFolderPathViewModel

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
      var levels = new List<TFolder>(folder.FolderToRoot<TFolder>().Reverse());
      var result = new List<FileSystemFolderOperations>(levels.Count);
      for (var i = 0; i < levels.Count; i++)
      {
        var f = levels[i];
        var nextFolder = i < levels.Count - 1 ? levels[i + 1] : null;
        var operation = new FileSystemFolderOperations(f, o => HandleNavigateTo(o, nextFolder));
        result.Add(operation);
      }

      SelectedFolders = result;
    }

    private void UpdateEmptySelectedFolders()
    {
      SelectedFolders = new[]
      {
        new FileSystemFolderOperations(_treeViewModel.Root.DataItem.CastTo<TFolder>(), o => HandleNavigateTo(o, null))
      };
    }

    private void HandleNavigateTo(IFileSystemFolderOperations operation, TFolder selected)
    {
      if (_gridSelectionToken != null)
      {
        _gridSelectionToken.Dispose();
      }

      var gvm = _gridViewModel;

      EventHandler handler = (s, e) => gvm.SetSelectedAsync(selected as TFile);

      var token = _gridSelectionToken = new DisposeManager();
      token.AddEvent(h => gvm.FilesPopulated += h, h => gvm.FilesPopulated -= h, handler);

      _treeViewModel.SelectAsync(operation.DriveFolder);
    }
    #endregion Private Methods
  }
}