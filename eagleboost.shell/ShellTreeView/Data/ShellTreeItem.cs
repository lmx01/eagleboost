using System.Linq;

namespace eagleboost.shell.ShellTreeView.Data
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Diagnostics;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.Extensions;
  using eagleboost.presentation.Collections;
  using eagleboost.shell.ShellTreeView.Contracts;
  using Microsoft.WindowsAPICodePack.Shell;

  [DebuggerDisplay("{Name}")]
  public partial class ShellTreeItem : NotifyPropertyChangedBase, IShellTreeItem, IEquatable<ShellTreeItem>
  {
    #region Statics

    public static readonly PropertyChangedEventArgs IsExpandedArgs = GetChangedArgs<ShellTreeItem>(o => o.IsExpanded);
    public static readonly PropertyChangedEventArgs IsSelectedArgs = GetChangedArgs<ShellTreeItem>(o => o.IsSelected);

    #endregion Statics

    private static readonly ShellTreeItem DummyChild = new ShellTreeItem("Dummy");

    private readonly ShellObject _shellObj;
    private readonly ShellContainer _shellContainer;
    private readonly ShellTreeContainerItem _parent;
    private readonly string _name;
    private readonly IShellItemsOperation _shellItemsOperation;
    private readonly ObservableCollection<IShellTreeItem> _children = new ObservableCollection<IShellTreeItem>();
    private readonly LiveListCollectionView _childrenView;
    private bool _isSelected;
    private bool _isExpanded;

    private ShellTreeItem(string name)
    {
      _name = name;
    }

    protected ShellTreeItem(ShellObject shellObj, ShellTreeContainerItem parent, IShellItemsOperation shellItemsOperation)
    {
      _shellObj = shellObj;
      _shellContainer = shellObj as ShellContainer;
      _parent = parent;
      _shellItemsOperation = shellItemsOperation;
      _childrenView =
        new LiveListCollectionView(_children)
        {
          Filter = o => shellItemsOperation.Filter((ShellTreeItem) o)
        }; // {FilterDescriptions = shellItemsOperation.FilterDescriptionCollection};
      if (_shellObj != null)
      {
        _name = _shellObj.Name;
      }

      if (_shellContainer != null)
      {
        _children.Add(DummyChild);
        PreloadThumbnails();
      }
    }

    public string Name
    {
      get { return _name; }
    }

    public ShellObject ShellObject
    {
      get { return _shellObj; }
    }

    private bool _isFiltered;
    public bool IsFiltered
    {
      get { return _isFiltered; }
      set { SetValue(ref _isFiltered, value); }
    }
    
    public bool IsExpanded
    {
      get { return _isExpanded; }
      set
      {
        SetValue(ref _isExpanded, value);

        // Expand all the way up to the root.
        if (_isExpanded && _parent != null)
        {
          _parent.IsExpanded = true;
        }

        if (HasDummyChild)
        {
          Children.Remove(DummyChild);
          LoadChildrenAsync();
        }
      }
    }

    public bool IsEmpty
    {
      get { return _childrenView.Count == 0; }
    }
    
    public bool IsChildrenLoaded { get; private set; }

    public bool HasDummyChild
    {
      get { return _shellContainer != null && Children.Count == 1 && Children[0] == DummyChild; }
    }

    public bool IsSelected
    {
      get { return _isSelected; }
      set { SetValue(ref _isSelected, value); }
    }

    public ShellTreeItem Parent
    {
      get { return _parent; }
    }

    public ObservableCollection<IShellTreeItem> Children
    {
      get { return _children; }
    }

    public LiveListCollectionView ChildrenView
    {
      get { return _childrenView; }
    }

    IReadOnlyCollection<IShellTreeItem> IShellTreeItem.Children
    {
      get { return Children; }
    }

    private void LoadChildrenAsync()
    {
      var items = _shellItemsOperation.CreateChildItems(_shellContainer.OrderBy(o => o.Name), (ShellTreeContainerItem) this);
      _children.AddRange(items);
    }

    private void PreloadThumbnails()
    {
      //Task.Run(() =>
      //{
      //  foreach (var i in _shellContainer)
      //  {
      //    var thumbnail = i.Thumbnail.SmallBitmapSource;
      //    if (thumbnail != null)
      //    {
      //      thumbnail = null;
      //    }
      //  }
      //});
    }

    public void Refresh()
    {
      if (IsChildrenLoaded)
      {
        _childrenView.Refresh();
      }
    }

    public bool Equals(ShellTreeItem other)
    {
      if (other == null)
      {
        return false;
      }

      return other.ShellObject == ShellObject;
    }

    public override int GetHashCode()
    {
      return _shellObj != null ? _shellObj.GetHashCode() ^ Name.GetHashCode() : Name.GetHashCode();
    }
  }
}