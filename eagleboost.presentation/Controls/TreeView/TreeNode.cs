﻿// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-19 11:51 PM

namespace eagleboost.presentation.Controls.TreeView
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using eagleboost.core.ComponentModel;
  using eagleboost.presentation.Collections;

  /// <summary>
  /// TreeNode
  /// </summary>
  public class TreeNode : NotifyPropertyChangedBase, ITreeNode, IEquatable<TreeNode>
  {
    #region Statics
    protected static readonly DummyTreeNode DummyChild = new DummyTreeNode();
    #endregion Statics

    #region Declarations
    private readonly object _dataItem;
    private readonly ITreeNodeContainer _parent;
    private readonly string _name;
    private readonly ITreeNodesOperation _treeNodesOperation;
    private readonly ObservableCollection<ITreeNode> _children = new ObservableCollection<ITreeNode>();
    private readonly LiveListCollectionView _childrenView;
    private bool _isSelected;
    private bool _isExpanded;
    private bool _isBeingExpanded;
    #endregion Declarations

    #region ctors
    protected TreeNode(string name)
    {
      _name = name;
    }

    protected TreeNode(object dataItem, ITreeNodeContainer parent, ITreeNodesOperation treeNodesOperation)
    {
      _dataItem = dataItem;
      _parent = parent;
      _treeNodesOperation = treeNodesOperation;
      _childrenView = new LiveListCollectionView(_children) {Filter = o => treeNodesOperation.Filter((ITreeNode) o)};
      _childrenView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

      _name = _dataItem is IDisplayItem ? ((IDisplayItem) _dataItem).DisplayName : _dataItem.ToString();
    }
    #endregion ctors

    #region Protected Properties
    protected ITreeNodesOperation TreeNodesOperation
    {
      get { return _treeNodesOperation; }
    }
    #endregion Protected Properties

    #region ITreeNode
    public string Name
    {
      get { return _name; }
    }

    public object DataItem
    {
      get { return _dataItem; }
    }

    public bool IsExpanded
    {
      get { return _isExpanded; }
      set
      {
        if (SetValue(ref _isExpanded, value))
        {
          // Expand all the way up to the root.
          if (_isExpanded && _parent != null)
          {
            _parent.IsExpanded = true;
          }

          OnIsExpandedChanged();
        }
      }
    }

    public bool IsBeingExpanded
    {
      get { return _isBeingExpanded; }
      protected set { SetValue(ref _isBeingExpanded, value); }
    }

    public bool IsEmpty
    {
      get { return _childrenView.Count == 0; }
    }

    public bool IsSelected
    {
      get { return _isSelected; }
      set
      {
        if (SetValue(ref _isSelected, value))
        {
          // Expand all the way up to the root.
          if (_isSelected && _parent != null)
          {
            _parent.IsExpanded = true;
          }
        }
      }
    }

    ITreeNode ITreeNode.Parent
    {
      get { return Parent; }
    }

    IReadOnlyCollection<ITreeNode> ITreeNode.Children
    {
      get { return Children; }
    }
    #endregion ITreeNode

    #region IEquatable
    public bool Equals(TreeNode other)
    {
      if (other == null)
      {
        return false;
      }

      return other.DataItem == DataItem;
    }

    public override int GetHashCode()
    {
      return _dataItem != null ? _dataItem.GetHashCode() ^ Name.GetHashCode() : Name.GetHashCode();
    }
    #endregion IEquatable

    #region Public Properties
    public virtual bool HasDummyChild
    {
      get { return false; }
    }

    public ITreeNodeContainer Parent
    {
      get { return _parent; }
    }

    public ObservableCollection<ITreeNode> Children
    {
      get { return _children; }
    }

    public LiveListCollectionView ChildrenView
    {
      get { return _childrenView; }
    }
    #endregion Public Properties

    #region Virtuals
    protected virtual void OnIsExpandedChanged()
    {
    }
    #endregion Virtuals

    #region Overrides
    public override string ToString()
    {
      return Name;
    }
    #endregion Overrides
  }
}