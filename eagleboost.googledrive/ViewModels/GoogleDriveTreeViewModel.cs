// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-20 11:32 PM

namespace eagleboost.googledrive.ViewModels
{
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Threading.Tasks;
  using System.Windows.Data;
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Models;
  using eagleboost.presentation.Contracts;
  using eagleboost.presentation.Controls.Indicators;
  using eagleboost.presentation.Controls.TreeView;
  using eagleboost.presentation.Tasks;
  using eagleboost.shell.FileSystems.Contracts;
  using eagleboost.shell.FileSystems.ViewModels;
  using Unity.Attributes;

  /// <summary>
  /// IGoogleDriveTreeViewModel
  /// </summary>
  public interface IGoogleDriveTreeViewModel : IFileSystemTreeViewModel
  {
  }

  public class GoogleDriveTreeViewModel : FileSystemTreeViewModel, IGoogleDriveTreeViewModel
  {
    #region Statics
    protected static readonly string TreeNodeName = Property<ITreeNode>(o => o.Name);
    #endregion Statics

    #region Declarations
    private IGoogleDriveFolder _rootFolder;
    private IGoogleDriveFolder _myDriveFolder;
    private IGoogleDriveFolder _teamDriveFolder;
    private IGoogleDriveService _gService;
    private readonly IDispatcherProvider _dispatcher;
    #endregion Declarations

    #region ctors
    public GoogleDriveTreeViewModel()
    {
      _dispatcher = new DispatcherProvider();
    }
    #endregion ctors

    #region Components
    [Dependency]
    public BusyStatusReceiver BusyStatusReceiver { get; set; }
    #endregion Components

    #region Init
    public void Initialize(IGoogleDriveService gService)
    {
      _gService = gService;
      _gService.FileCreated += HangleGoogleDriveFileCreated;
    }
    #endregion Init

    #region Overrides
    protected override ICollectionView CreateItemsView()
    {
      var result = new ListCollectionView(Items) { Filter = o => Filter((ITreeNode)o) };
      result.SortDescriptions.Add(new SortDescription(TreeNodeName, ListSortDirection.Ascending));
      return result;
    }

    protected override bool DoFilter(ITreeNode node)
    {
      return node is DummyTreeNode || node.DataItem is IGoogleDriveFolder;
    }
    protected override ITreeNodeContainer CreateRootNode()
    {
      var root = new GoogleDriveRoot();
      var myDrive = new GoogleMyDrive();
      var teamDrive = new GoogleTeamDrive();

      _rootFolder = new GoogleDriveFolder(root, null, GetRootsAsync);
      _myDriveFolder = new GoogleDriveFolder(myDrive, _rootFolder, GetFilesAsync);
      _teamDriveFolder = new GoogleDriveFolder(teamDrive, _rootFolder, GetTeamDrivesAsync);
      return new TreeNodeContainer(_rootFolder, null, this);
    }
    #endregion Overrides

    #region Private Methods
    private Task<IReadOnlyList<IGoogleDriveFile>> GetRootsAsync(GoogleDriveFolder parent)
    {
      IReadOnlyList<IGoogleDriveFile> roots = new IGoogleDriveFile[] {_myDriveFolder, _teamDriveFolder};
      return Task.FromResult(roots);
    }

    private Task<IReadOnlyList<IGoogleDriveFile>> GetFilesAsync(GoogleDriveFolder parent)
    {
      return _gService.GetChildFilesAsync(parent, progress: BusyStatusReceiver);
    }

    private Task<IReadOnlyList<IGoogleDriveFile>> GetTeamDrivesAsync(GoogleDriveFolder parent)
    {
      return _gService.GetTeamDrivesAsync(parent, progress: BusyStatusReceiver);
    }
    #endregion Private Methods

    #region Event Handlers
    private void HangleGoogleDriveFileCreated(object sender, GoogldDriveFileCreatedEventArgs args)
    {
      var folder = args.File as IGoogleDriveFolder;
      if (folder != null)
      {
        var parentNode = (ITreeNodeContainer)FindNode(folder.Parent);
        ////Do nothing if we can't find the parent Node - it's not created yet so don't bother add data to its children
        if (parentNode != null && parentNode.IsExpanded)
        {
          _dispatcher.BeginInvoke(() => parentNode.AddData(folder));
        }
      }
    }
    #endregion Event Handlers
  }
}