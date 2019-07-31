// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-30 9:24 PM

namespace eagleboost.shell.FileSystems.ViewModels
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Linq;
  using System.Threading.Tasks;
  using System.Windows.Data;
  using eagleboost.core.Collections;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.Extensions;
  using eagleboost.core.Logging;
  using eagleboost.presentation.Collections;
  using eagleboost.presentation.Controls.TreeView;
  using eagleboost.shell.FileSystems.Contracts;

  /// <summary>
  /// FileSystemTreeViewModel
  /// </summary>
  public abstract class FileSystemTreeViewModel : NotifyPropertyChangedBase, IFileSystemTreeViewModel
  {
    #region Statics
    private static readonly ILoggerFacade Logger = LoggerManager.GetLogger<FileSystemTreeViewModel>();
    protected static readonly string Name = Property<IFile>(o => o.Name);
    protected static readonly PropertyChangedEventArgs SelectedItemArgs = GetChangedArgs<FileSystemTreeViewModel>(o => o.SelectedItem);
    #endregion Statics

    #region Declarations
    private readonly ObservableCollection<ITreeNode> _items = new ObservableCollection<ITreeNode>();
    private ICollectionView _itemsView;
    private ITreeNode _selectedItem;
    private ITreeNodeContainer _root;
    #endregion Declarations

    #region ctors
    protected FileSystemTreeViewModel()
    {
      SelectionContainer = new SingleSelectionContainer<ITreeNode>();
    }
    #endregion ctors

    #region Components
    public SingleSelectionContainer<ITreeNode> SelectionContainer { get; private set; }
    #endregion Components

    #region IFileSystemTreeViewModel
    ISelectionContainer<ITreeNode> ICollectionViewModel<ITreeNode>.SelectionContainer
    {
      get { return SelectionContainer; }
    }

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
      get { return _itemsView ?? (_itemsView = CreateItemsView()); }
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
      get { return (IList)SelectionContainer.SelectedItems; }
    }

    public event EventHandler SelectedItemsChanged;

    public ITreeNode Root
    {
      get { return _root; }
    }

    public bool LoadOnSelectionChange { get; set; }

    public async Task<bool> SelectAsync(params string[] paths)
    {
      var processedPaths = ProcessPaths(paths);
      var node = (ITreeNodeContainer)Root;
      var folderStack = new Stack<string>(processedPaths.Reverse());
      folderStack.Pop();
      if (folderStack.Count == 0)
      {
        return false;
      }

      while (folderStack.Count > 0)
      {
        await node.LoadChildrenAsync().ConfigureAwait(true);

        var folder = folderStack.Pop();
        var childNode = node.Children.FirstOrDefault(i => folder == i.DataItem.CastTo<IFile>().Name);
        if (childNode != null)
        {
          node = (ITreeNodeContainer)childNode;
        }
      }

      SelectedItem = node;
      return node != null;
    }
    #endregion IFileSystemTreeViewModel

    #region IFileSystemTreeNodesOperation
    public Task<ITreeNode> InitializeAsync(bool isSingleRoot)
    {
      return DoCreateRootAsync(isSingleRoot);
    }

    public Task<ITreeNode> CreateChildAsync(object childDataItem, ITreeNodeContainer parent)
    {
      return DoCreateChildAsync(childDataItem, parent);
    }

    public Task<IReadOnlyCollection<ITreeNode>> CreateChildrenAsync(object parentDataItem, ITreeNodeContainer parent)
    {
      return DoCreateChildrenAsync(parentDataItem, parent);
    }

    public bool Filter(ITreeNode item)
    {
      return DoFilter(item);
    }
    #endregion IFileSystemTreeNodesOperation

    #region Virtuals
    protected virtual IEnumerable<string> ProcessPaths(string[] paths)
    {
      return paths;
    }

    protected abstract bool DoFilter(ITreeNode item);

    protected abstract ITreeNodeContainer CreateRootNode();

    protected virtual TreeNodeContainer CreateContainerNode(IFile f, ITreeNodeContainer parent)
    {
      return new TreeNodeContainer(f, parent, this);
    }

    protected virtual TreeNodeData CreateDataNode(IFile f, ITreeNodeContainer parent)
    {
      return new TreeNodeData(f, parent, this);
    }

    protected virtual ICollectionView CreateItemsView()
    {
      return new ListCollectionView(_items) {Filter = o => Filter((ITreeNode) o)};
    }
    #endregion Virtuals

    #region Overrides
    protected override void OnPropertyChanged(string propertyName)
    {
      base.OnPropertyChanged(propertyName);

      if (SelectedItemArgs.Match(propertyName))
      {
        var selected = SelectedItem as ITreeNodeContainer;
        if (selected != null)
        {
          selected.IsSelected = true;
          if (LoadOnSelectionChange)
          {
            selected.LoadChildrenAsync().ConfigureAwait(false);
          }
        }
      }
    }
    #endregion Overrides

    #region Protected Methods
    protected ITreeNode FindNode(IFile file)
    {
      return null;
    }
    #endregion Protected Methods

    #region Private Methods
    private async Task<ITreeNode> DoCreateRootAsync(bool isSingleRoot)
    {
      var root = _root = CreateRootNode();
      await root.ExpandAsync().ConfigureAwait(true);
      if (isSingleRoot)
      {
        Items.Add(root);
      }
      else
      {
        Items.AddRange(root.Children);
      }

      return root;
    }

    protected async Task<IReadOnlyCollection<ITreeNode>> DoCreateChildrenAsync(object parentItem, ITreeNodeContainer parent)
    {
      var folder = parentItem as IFolder;
      if (folder != null)
      {
        var files = await folder.GetFilesAsync().ConfigureAwait(true);
        return files.Select(f => CreateTreeNode(f, parent)).ToArray();
      }

      return new ITreeNode[0];
    }

    protected Task<ITreeNode> DoCreateChildAsync(object childDataItem, ITreeNodeContainer parent)
    {
      var file = (IFile)childDataItem;
      return Task.FromResult(CreateTreeNode(file, parent));
    }

    private ITreeNode CreateTreeNode(IFile f, ITreeNodeContainer parent)
    {
      if (f is IFolder)
      {
        return CreateContainerNode(f, parent);
      }

      return CreateDataNode(f, parent);
    }
    #endregion Private Methods
  }
}