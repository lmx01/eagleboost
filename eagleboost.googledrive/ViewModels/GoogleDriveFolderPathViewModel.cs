// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-26 6:42 PM

namespace eagleboost.googledrive.ViewModels
{
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using eagleboost.core.ComponentModel;
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
    private IReadOnlyList<IGoogleDriveFolderOperations> _selectedFolders;
    #endregion Declarations

    #region ctors
    public GoogleDriveFolderPathViewModel(IGoogleDriveTreeViewModel treeViewModel)
    {
      _treeViewModel = treeViewModel;
      _treeViewModel.PropertyChanged += HandleTreePropertyChanged;
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
      var result = levels.Select(f => new GoogleDriveFolderOperations(f, HandleNavigateTo));
      SelectedFolders = result.ToArray();
    }

    private void UpdateEmptySelectedFolders()
    {
      SelectedFolders = new []{ new GoogleDriveFolderOperations(_treeViewModel.Root.DataItem.CastTo<IGoogleDriveFolder>(), HandleNavigateTo) };
    }

    private void HandleNavigateTo(IGoogleDriveFolderOperations operation)
    {
      _treeViewModel.SelectAsync(operation.DriveFolder);
    }
    #endregion Private Methods
  }
}