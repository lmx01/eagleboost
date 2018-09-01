// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-30 9:24 PM

namespace eagleboost.shell.FileSystems.TreeView
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Linq;
  using System.Threading.Tasks;
  using System.Windows.Data;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.Extensions;
  using eagleboost.presentation.Collections;
  using eagleboost.presentation.Controls.TreeView;
  using eagleboost.shell.FileSystems.Contracts;

  /// <summary>
  /// FileSystemTreeViewModel
  /// </summary>
  public abstract class FileSystemTreeViewModel : NotifyPropertyChangedBase, IFileSystemTreeViewModel
  {
    #region Statics
    protected static readonly string Name = Property<IFile>(o => o.Name);
    protected static readonly PropertyChangedEventArgs SelectedItemArgs = GetChangedArgs<FileSystemTreeViewModel>(o => o.SelectedItem);
    #endregion Statics

    #region Declarations
    private readonly ObservableCollection<ITreeNode> _items = new ObservableCollection<ITreeNode>();
    private ICollectionView _itemsView;
    private ITreeNode _selectedItem;
    private TreeNodeContainer _root;
    #endregion Declarations

    #region IFileSystemTreeViewModel
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
      get { throw new NotImplementedException(); }
      set { throw new NotImplementedException(); }
    }

    public event EventHandler SelectedItemsChanged;

    public ITreeNode Root
    {
      get { return _root; }
    }

    public async Task<bool> SelectAsync(params string[] path)
    {
      var node = (TreeNodeContainer)Root;
      var folderStack = new Stack<string>(path.Reverse());
      folderStack.Pop();
      while (folderStack.Count > 0)
      {
        await node.LoadChildrenAsync().ConfigureAwait(true);

        var folder = folderStack.Pop();
        var childNode = node.Children.FirstOrDefault(i => folder == i.DataItem.CastTo<IFile>().Name);
        if (childNode != null)
        {
          node = (TreeNodeContainer)childNode;
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

    public Task<IReadOnlyList<ITreeNode>> CreateChildrenAsync(object parentDataItem, TreeNodeContainer parent)
    {
      return DoCreateChildrenAsync(parentDataItem, parent);
    }

    public bool Filter(ITreeNode item)
    {
      return DoFilter(item);
    }
    #endregion IFileSystemTreeNodesOperation

    #region Virtuals
    protected abstract bool DoFilter(ITreeNode item);

    protected abstract TreeNodeContainer CreateRootNode();

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
        var selected = (TreeNodeContainer)SelectedItem;
        if (selected != null)
        {
          selected.IsSelected = true;
          selected.LoadChildrenAsync().ConfigureAwait(false);
        }
      }
    }
    #endregion Overrides

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

    protected async Task<IReadOnlyList<ITreeNode>> DoCreateChildrenAsync(object parentItem, TreeNodeContainer parent)
    {
      var folder = parentItem as IFolder;
      if (folder != null)
      {
        var files = await folder.GetFilesAsync().ConfigureAwait(true);
        return files.Select(f => CreateTreeNode(f, parent)).ToArray();
      }

      return new ITreeNode[0];
    }

    private ITreeNode CreateTreeNode(IFile f, TreeNodeContainer parent)
    {
      if (f is IFolder)
      {
        return new TreeNodeContainer(f, parent, this);
      }

      return new TreeNodeData(f, parent, this);
    }
    #endregion Private Methods
  }
}