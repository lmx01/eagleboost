// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-24 12:58 AM

namespace eagleboost.googledrive.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reactive.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.core.Collections;
  using eagleboost.core.Extensions;
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Extensions;
  using eagleboost.googledrive.Models;
  using eagleboost.presentation.Controls.TreeView;
  using eagleboost.shell.FileSystems.Contracts;
  using eagleboost.shell.FileSystems.ViewModels;

  /// <summary>
  /// IGoogleDriveGridViewModel
  /// </summary>
  public interface IGoogleDriveGridViewModel : IFileSystemCollectionViewModel<IGoogleDriveFile, IGoogleDriveFolder>
  {
  }

  /// <summary>
  /// GoogleDriveGridViewModel
  /// </summary>
  public class GoogleDriveGridViewModel: FileSystemCollectionViewModel<IGoogleDriveFile, IGoogleDriveFolder>, IGoogleDriveGridViewModel
  {
    #region Statics
    private static readonly IReadOnlyCollection<IGoogleDriveFile> Empty = Array<IGoogleDriveFile>.Empty;
    #endregion Statics

    #region Declarations
    private IGoogleDriveService _gService;
    #endregion Declarations

    #region Public Methods
    public void Initialize(IGoogleDriveService gService)
    {
      _gService = gService;
    }
    #endregion Public Methods

    #region Overrides
    protected override Task<IReadOnlyCollection<IGoogleDriveFile>> PopulateFolderAsync(ITreeNodeContainer folderNode, CancellationToken ct, IProgress<string> progress)
    {
      if (!folderNode.ChildrenTask.IsCompleted)
      {
        var parent = folderNode.CastTo<TreeNodeContainer>().DataItem.CastTo<IGoogleDriveFolder>();
        if (parent.IsMyDriveFile() || (parent.File.IsNot<GoogleTeamDrive>() && parent.IsTeamDriveFile()))
        {
          Items.Clear();

          var observable = _gService.GetChildFilesObservable(parent, ct: ct);
          observable.Buffer(TimeSpan.FromMilliseconds(400))
            .ObserveOn(SynchronizationContext.Current)
            .Subscribe(lf => Items.AddRange(lf.SelectMany(i => i)), RaiseFilesPopulated);

          return Task.FromResult(Empty);
        }
      }

      return base.PopulateFolderAsync(folderNode, ct, progress);
    }

    protected override bool ShouldUseChildrenTask(ITreeNodeContainer node)
    {
      var folder = (IGoogleDriveFolder)node.DataItem;
      return folder.File.IsNot<GoogleAdhocDrive>() && folder.File.IsNot<GoogleSearchResultDrive>();
    }
    #endregion Overrides
  }
}