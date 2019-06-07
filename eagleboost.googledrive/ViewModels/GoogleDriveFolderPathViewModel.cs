// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-26 6:42 PM

namespace eagleboost.googledrive.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.Data;
  using eagleboost.core.Extensions;
  using eagleboost.googledrive.Contracts;
  using eagleboost.presentation.Collections;
  using eagleboost.presentation.Controls.TreeView;
  using eagleboost.shell.FileSystems.Extensions;

  /// <summary>
  /// IGoogleDriveFolderPathViewModel
  /// </summary>
  public interface IGoogleDriveFolderPathViewModel
  {
    IReadOnlyList<IGoogleDriveFolderOperations> SelectedFolders { get; }
  }

  /// <summary>
  /// GoogleDriveFolderPathViewModel
  /// </summary>
  public class GoogleDriveFolderPathViewModel : NotifyPropertyChangedBase, IGoogleDriveFolderPathViewModel
  {
    #region Declarations
    private readonly IGoogleDriveTreeViewModel _treeViewModel;
    private readonly IGoogleDriveGridViewModel _gridViewModel;
    private IReadOnlyList<IGoogleDriveFolderOperations> _selectedFolders;
    private DisposeManager _gridSelectionToken;
    #endregion Declarations

    #region ctors
    public GoogleDriveFolderPathViewModel(IGoogleDriveTreeViewModel treeViewModel, IGoogleDriveGridViewModel gridViewModel)
    {
      _treeViewModel = treeViewModel;
      _treeViewModel.PropertyChanged += HandleTreePropertyChanged;
      _gridViewModel = gridViewModel;
    }
    #endregion ctors

    #region IGoogleDriveFolderPathViewModel
    public IReadOnlyList<IGoogleDriveFolderOperations> SelectedFolders
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
        var selected = (TreeNodeContainer) _treeViewModel.SelectedItem;
        if (selected != null)
        {
          UpdateSelectedFolders((IGoogleDriveFolder) selected.DataItem);
        }
        else
        {
          UpdateEmptySelectedFolders();
        }
      }
    }
    #endregion Event Handlers

    #region Private Methods
    private void UpdateSelectedFolders(IGoogleDriveFolder folder)
    {
      var levels = new List<IGoogleDriveFolder>(folder.FolderToRoot<IGoogleDriveFolder>().Reverse());
      var result = new List<GoogleDriveFolderOperations>(levels.Count);
      for (var i = 0; i < levels.Count; i++)
      {
        var f = levels[i];
        var nextFolder = i < levels.Count - 1 ? levels[i + 1] : null;
        var operation = new GoogleDriveFolderOperations(f, o => HandleNavigateTo(o, nextFolder));
        result.Add(operation);
      }

      SelectedFolders = result;
    }

    private void UpdateEmptySelectedFolders()
    {
      SelectedFolders = new[]
      {
        new GoogleDriveFolderOperations(_treeViewModel.Root.DataItem.CastTo<IGoogleDriveFolder>(), o => HandleNavigateTo(o, null))
      };
    }

    private void HandleNavigateTo(IGoogleDriveFolderOperations operation, IGoogleDriveFolder selected)
    {
      if (_gridSelectionToken != null)
      {
        _gridSelectionToken.Dispose();
      }

      var gvm = _gridViewModel;

      EventHandler handler = (s, e) => gvm.SetSelectedAsync(selected);

      var token = _gridSelectionToken = new DisposeManager();
      token.AddEvent(h => gvm.FilesPopulated += h, h => gvm.FilesPopulated -= h, handler);

      _treeViewModel.SelectAsync(operation.DriveFolder);
    }
    #endregion Private Methods
  }
}