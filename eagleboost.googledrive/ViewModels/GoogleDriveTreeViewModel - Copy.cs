// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-20 11:32 PM

namespace eagleboost.googledrive.ViewModels
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Linq;
  using System.Threading.Tasks;
  using System.Windows.Data;
  using System.Windows.Threading;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.Extensions;
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Extensions;
  using eagleboost.googledrive.Models;
  using eagleboost.googledrive.Services;
  using eagleboost.presentation.Collections;
  using eagleboost.presentation.Controls.Indicators;
  using eagleboost.presentation.Controls.TreeView;
  using Google.Apis.Drive.v3.Data;

  /// <summary>
  /// IGoogleDriveTreeViewModel
  /// </summary>
  public interface IGoogleDriveTreeViewModel : ICollectionViewModel<ITreeNode>, INotifyPropertyChanged
  {
    #region Properties
    ITreeNode Root { get; }
    #endregion Properties

    #region Methods
    Task<ITreeNode> InitializeAsync(bool expandRoot = true);

    Task<bool> SelectAsync(IGoogleDriveFolder toSelect);

    Task<bool> SelectAsync(string[] path);
    #endregion Methods
  }

  /// <summary>
  /// GoogleDriveTreeViewModel
  /// </summary>
  public class GoogleDriveTreeViewModel : NotifyPropertyChangedBase, IGoogleDriveTreeViewModel, ITreeNodesOperation
  {
    #region Declarations
    private TreeNodeContainer _rootNode;
    private IGoogleDriveFolder _rootFolder;
    private readonly ObservableCollection<ITreeNode> _items = new ObservableCollection<ITreeNode>();
    private readonly ListCollectionView _itemsView;
    private ITreeNode _selectedItem;
    private readonly IGoogleDriveService _gService;
    private readonly File _rootFile = new File {Id = "root", Name = "My Drive", OwnedByMe = true};
    private readonly Dispatcher _diapatcher = Dispatcher.CurrentDispatcher;
    #endregion Declarations

    #region ctors
    public GoogleDriveTreeViewModel(string credentialFileName, string applicationName)
    {
      var view = _itemsView = new ListCollectionView(_items);
      view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
      _gService = new GoogleDriveService(credentialFileName, applicationName);
    }
    #endregion ctors

    #region IGoogleDriveTreeViewModel
    IList ICollectionViewModel.Items
    {
      get { return Items; }
    }

    public ObservableCollection<ITreeNode> Items
    {
      get { return _items; }
    }

    public ICollectionView ItemsView
    {
      get { return _itemsView; }
    }

    object ICollectionViewModel.SelectedItem
    {
      get { return SelectedItem; }
      set { SelectedItem = (ITreeNode)value; }
    }

    public ITreeNode SelectedItem
    {
      get { return _selectedItem; }
      set { SetValue(ref _selectedItem, value); }
    }

    public IList SelectedItems
    {
      get { throw new NotImplementedException(); }
      set { throw new NotImplementedException(); }
    }

    public event EventHandler SelectedItemsChanged;

    public ITreeNode Root
    {
      get { return _rootNode; }
    }

    public async Task<ITreeNode> InitializeAsync(bool expandRoot = true)
    {
      var root = _rootNode = CreateRootNode();
      _items.Add(root);
      if (expandRoot)
      {
        await root.ExpandAsync().ConfigureAwait(true);
      }
      
      return root;
    }

    public async Task<bool> SelectAsync(IGoogleDriveFolder toSelect)
    {
      var node = _rootNode;
      var folderStack = new Stack<IGoogleDriveFolder>(toSelect.FolderToRoot());
      folderStack.Pop();
      while (folderStack.Count > 0)
      {
        await node.LoadChildrenAsync().ConfigureAwait(true);

        var folder = folderStack.Pop();
        var childNode = node.Children.FirstOrDefault(i => folder.Id == i.DataItem.CastTo<IGoogleDriveFolder>().Id);
        if (childNode != null)
        {
          node = (TreeNodeContainer)childNode;
        }
      }

      SelectedItem = node;
      return node != null;
    }

    public async Task<bool> SelectAsync(params string[] path)
    {
      var node = _rootNode;
      var folderStack = new Stack<string>(path.Reverse());
      folderStack.Pop();
      while (folderStack.Count > 0)
      {
        await node.LoadChildrenAsync().ConfigureAwait(true);

        var folder = folderStack.Pop();
        var childNode = node.Children.FirstOrDefault(i => folder == i.DataItem.CastTo<IGoogleDriveFile>().Name);
        if (childNode != null)
        {
          node = (TreeNodeContainer)childNode;
        }
      }

      SelectedItem = node;
      return node != null;
    }

    #endregion IGoogleDriveTreeViewModel

    #region Public Properties
    public BusyStatusReceiver BusyStatusReceiver { get; set; }
    #endregion Public Properties

    #region Private Methods
    private TreeNodeContainer CreateRootNode()
    {
      _rootFolder = new GoogleDriveFolder(_rootFile, null, f => _gService.GetChildFilesAsync(f, progress: BusyStatusReceiver));
      return new TreeNodeContainer(_rootFolder, null, this);
    }

    private ITreeNode CreateTreeNode(IGoogleDriveFile f, TreeNodeContainer parent)
    {
      if (f is IGoogleDriveFolder)
      {
        return new TreeNodeContainer(f, parent, this);
      }

      return new TreeNodeData(f, parent, this);
    }
    #endregion Private Methods

    #region ITreeNodesOperation
    public async Task<IReadOnlyList<ITreeNode>> CreateChildrenAsync(object parentDataItem, TreeNodeContainer parent)
    {
      var folder = parentDataItem as GoogleDriveFolder;
      if (folder != null)
      {
        var files = await folder.GetFilesAsync().ConfigureAwait(true);
        return files.Select(f => CreateTreeNode(f, parent)).ToArray();
      }

      return new ITreeNode[0];
    }

    public bool Filter(ITreeNode node)
    {
      return node is DummyTreeNode || node.DataItem is IGoogleDriveFolder;
    }
    #endregion ITreeNodesOperation

    #region Overrides

    protected override void OnPropertyChanged(string propertyName)
    {
      base.OnPropertyChanged(propertyName);

      if (propertyName == "SelectedItem")
      {
        var selected = (TreeNodeContainer) SelectedItem;
        if (selected != null)
        {
          selected.IsSelected = true;
          selected.LoadChildrenAsync().ConfigureAwait(false);
        }
      }
    }
    #endregion Overrides
  }
}