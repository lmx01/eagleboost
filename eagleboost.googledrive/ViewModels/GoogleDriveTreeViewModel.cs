// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-20 11:32 PM

namespace eagleboost.googledrive.ViewModels
{
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
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
    private IGoogleDriveFolder _activityFolder;
    private IGoogleDriveFolder _adhocFolder;
    private ITreeNode _activityNode;
    private ITreeNode _adhocNode;
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
      _gService.FileCreated += HandleGoogleDriveFileCreated;
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
      if (node is DummyTreeNode || node.DataItem is IGoogleDriveFolder)
      {
        return true;
      }

      if (node.Parent == AdhocNode)
      {
        return true;
      }

      return false;
    }

    protected override ITreeNodeContainer CreateRootNode()
    {
      var root = new GoogleDriveRoot();
      var myDrive = new GoogleMyDrive();
      var teamDrive = new GoogleTeamDrive();
      var activityDrive = new GoogleActivityDrive();
      var adhocDrive = new GoogleAdhocDrive();

      _rootFolder = new GoogleDriveFolder(root, null, GetRootsAsync);
      _myDriveFolder = new GoogleDriveFolder(myDrive, _rootFolder, GetFilesAsync);
      _teamDriveFolder = new GoogleDriveFolder(teamDrive, _rootFolder, GetTeamDrivesAsync);
      _activityFolder = new GoogleDriveFolder(activityDrive, _rootFolder, GetActivityFilesAsync);
      _adhocFolder = new GoogleDriveFolder(adhocDrive, _rootFolder, GetAdhocFilesAsync);
      return new TreeNodeContainer(_rootFolder, null, this);
    }

    protected override TreeNodeContainer CreateContainerNode(IFile f, ITreeNodeContainer parent)
    {
      var result = new TreeNodeContainer(f, parent, this);
      if (Equals(f, _myDriveFolder)|| Equals(f, _teamDriveFolder))
      {
        result.SortProperty = "Name";
      }

      return result;
    }
    #endregion Overrides

    #region Public Methods
    public async void AddAdhocFile(IGoogleDriveFile file)
    {
      var adhocNode = AdhocNode as TreeNodeContainer;
      if (adhocNode != null)
      {
        if (adhocNode.HasDummyChild)
        {
          await adhocNode.ExpandAsync().ConfigureAwait(true);
        }

        _dispatcher.CheckedInvoke(() => adhocNode.AddData(file));
      }
    }
    #endregion Public Methods

    #region Private Properties
    private ITreeNode ActivityNode
    {
      get { return _activityNode ?? (_activityNode = Root.Children.FirstOrDefault(i => i.DataItem == _activityFolder)); }
    }

    private ITreeNode AdhocNode
    {
      get { return _adhocNode ?? (_adhocNode = Root.Children.FirstOrDefault(i => i.DataItem == _adhocFolder)); }
    }
    #endregion Private Properties

    #region Private Methods
    private Task<IReadOnlyList<IGoogleDriveFile>> GetRootsAsync(GoogleDriveFolder parent)
    {
      IReadOnlyList<IGoogleDriveFile> roots = new IGoogleDriveFile[] {_myDriveFolder, _teamDriveFolder, _activityFolder, _adhocFolder };
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

    private Task<IReadOnlyList<IGoogleDriveFile>> GetActivityFilesAsync(GoogleDriveFolder parent)
    {
      return _gService.GetActivityFilesAsync(parent, progress: BusyStatusReceiver);
    }

    private Task<IReadOnlyList<IGoogleDriveFile>> GetAdhocFilesAsync(GoogleDriveFolder parent)
    {
      var adhocNode = AdhocNode as TreeNodeContainer;
      if (adhocNode != null && !adhocNode.HasDummyChild)
      {
        IReadOnlyList<IGoogleDriveFile> files = adhocNode.Children.Select(i => i.DataItem).Cast<IGoogleDriveFile>().ToArray();
        return Task.FromResult(files);
      }

      IReadOnlyList<IGoogleDriveFile> emptyFiles = new IGoogleDriveFile[0];
      return Task.FromResult(emptyFiles);
    }
    #endregion Private Methods

    #region Event Handlers
    private void HandleGoogleDriveFileCreated(object sender, GoogldDriveFileCreatedEventArgs args)
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