// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-20 11:32 PM

namespace eagleboost.googledrive.ViewModels
{
  using System.Collections;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using System.Threading.Tasks;
  using System.Windows.Data;
  using eagleboost.core.Collections;
  using eagleboost.core.Extensions;
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Extensions;
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

  /// <summary>
  /// GoogleDriveTreeViewModel
  /// </summary>
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
    private IGoogleDriveFolder _searchResultFolder;
    private ITreeNode _activityNode;
    private ITreeNode _adhocNode;
    private ITreeNode _searchResultNode;
    private IGoogleDriveService _gService;
    private readonly IDispatcherProvider _dispatcher;
    private PriorityComparer<TreeNodeContainer> _favoriteFileComparer;
    private GoogleDriveFileNameComparer _fileNameComparer;
    private readonly Dictionary<string, int> _filePriority = new Dictionary<string, int>();
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

    #region Public Properties
    public bool UpdateFrequentFolder { get; set; }
    #endregion Public Properties

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

      if (node.Parent == AdhocNode || node.Parent == SearchResultNode)
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
      var searchResultDrive = new GoogleSearchResultDrive();

      _rootFolder = new GoogleDriveFolder(root, null, GetRootsAsync);
      _myDriveFolder = new GoogleDriveFolder(myDrive, _rootFolder, GetFilesAsync);
      _teamDriveFolder = new GoogleDriveFolder(teamDrive, _rootFolder, GetTeamDrivesAsync);
      _activityFolder = new GoogleDriveFolder(activityDrive, _rootFolder, GetActivityFilesAsync);
      _adhocFolder = new GoogleDriveFolder(adhocDrive, _rootFolder, GetAdhocFilesAsync);
      _searchResultFolder = new GoogleDriveFolder(searchResultDrive, _rootFolder, GetSearchResultAsync);
      return new TreeNodeContainer(_rootFolder, null, this);
    }

    protected override TreeNodeContainer CreateContainerNode(IFile f, ITreeNodeContainer parent)
    {
      var result = new TreeNodeContainer(f, parent, this);
      if (Equals(f, _myDriveFolder) || Equals(f, _teamDriveFolder))
      {
        if (UpdateFrequentFolder)
        {
          result.CustomSort = new CompositeComparer<TreeNodeContainer>(new[] {FavoriteFileComparer, FileNameComparer});
        }
        else
        {
          result.CustomSort = (IComparer)FileNameComparer;
        }
      }

      return result;
    }
    #endregion Overrides

    #region Public Methods
    public async void AddAdhocFileAsync(IGoogleDriveFile file)
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

    public async void SetSearchResultAsync(IEnumerable<IGoogleDriveFile> files)
    {
      var node = SearchResultNode as TreeNodeContainer;
      if (node != null)
      {
        if (node.HasDummyChild)
        {
          await node.ExpandAsync().ConfigureAwait(true);
        }

        _dispatcher.CheckedInvoke(() =>
        {
          node.Clear();
          foreach (var file in files)
          {
            node.AddData(file);
          }
        });
      }
    }

    public void RefreshCurrent()
    {
      var selected = SelectedItem;
      var c= selected.Parent as TreeNodeContainer;
      if (c != null)
      {
        c.ChildrenView.Refresh();
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

    private ITreeNode SearchResultNode
    {
      get { return _searchResultNode ?? (_searchResultNode = Root.Children.FirstOrDefault(i => i.DataItem == _searchResultFolder)); }
    }

    private IComparer<TreeNodeContainer> FavoriteFileComparer
    {
      get { return _favoriteFileComparer ?? (_favoriteFileComparer = CreateFavoriteFileComparer()); }
    }

    private IComparer<TreeNodeContainer> FileNameComparer
    {
      get { return _fileNameComparer ?? (_fileNameComparer = CreateFileNameComparer()); }
    }
    #endregion Private Properties

    #region Overrides
    protected override void OnPropertyChanged(string propertyName)
    {
      base.OnPropertyChanged(propertyName);

      if (SelectedItemArgs.Match(propertyName))
      {
        var selected = SelectedItem;
        if (selected != null)
        {
          UpdatePriority(selected as TreeNodeContainer);
        }
      }
    }
    #endregion Overrides

    #region Private Methods
    private PriorityComparer<TreeNodeContainer> CreateFavoriteFileComparer()
    {
      return new PriorityComparer<TreeNodeContainer>(GetPriority);
    }

    private GoogleDriveFileNameComparer CreateFileNameComparer()
    {
      return new GoogleDriveFileNameComparer();
    }

    private void UpdatePriority(TreeNodeContainer n)
    {
      var f = (IGoogleDriveFile) n.DataItem;

      int priority;
      if (_filePriority.TryGetValue(f.Id, out priority))
      {
        _filePriority[f.Id] = ++priority;
      }
      else
      {
        _filePriority[f.Id] = 1;
      }
    }

    private int GetPriority(TreeNodeContainer n)
    {
      if (n == null)
      {
        return 0;
      }

      var f = (IGoogleDriveFile) n.DataItem;
      if (f == null)
      {
        return 0;
      }

      int priority;
      if (_filePriority.TryGetValue(f.Id, out priority))
      {
        return priority;
      }

      return 0;
    }

    private Task<IReadOnlyList<IGoogleDriveFile>> GetRootsAsync(GoogleDriveFolder parent)
    {
      IReadOnlyList<IGoogleDriveFile> roots = new IGoogleDriveFile[] {_myDriveFolder, _teamDriveFolder, _activityFolder, _adhocFolder, _searchResultFolder };
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

      IReadOnlyList<IGoogleDriveFile> emptyFiles = Array<IGoogleDriveFile>.Empty;;
      return Task.FromResult(emptyFiles);
    }

    private Task<IReadOnlyList<IGoogleDriveFile>> GetSearchResultAsync(GoogleDriveFolder parent)
    {
      var searchResultNode = SearchResultNode as TreeNodeContainer;
      if (searchResultNode != null && !searchResultNode.HasDummyChild)
      {
        IReadOnlyList<IGoogleDriveFile> files = searchResultNode.Children.Select(i => i.DataItem).Cast<IGoogleDriveFile>().ToArray();
        return Task.FromResult(files);
      }

      IReadOnlyList<IGoogleDriveFile> emptyFiles = Array<IGoogleDriveFile>.Empty;;
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