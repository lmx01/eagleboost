﻿// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-19 11:53 PM

namespace eagleboost.presentation.Controls.TreeView
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Threading.Tasks;
  using System.Windows.Threading;
  using eagleboost.presentation.Extensions;

  /// <summary>
  /// TreeNodeContainer
  /// </summary>
  public class TreeNodeContainer : TreeNode
  {
    #region Declarations
    private TaskCompletionSource<IReadOnlyList<ITreeNode>> _childrenLoadedTcs;
    private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;
    #endregion Declarations

    #region ctors
    public TreeNodeContainer(object dataItem, TreeNodeContainer parent, ITreeNodesOperation shellItemsOperation) : base(dataItem, parent, shellItemsOperation)
    {
      Children.Add(DummyChild);
    }
    #endregion ctors

    #region Public Methods
    public async Task ExpandAsync()
    {
      if (!IsExpanded)
      {
        await LoadChildrenAsync().ConfigureAwait(true);
        IsExpanded = true;
      }
    }

    public Task LoadChildrenAsync()
    {
      _dispatcher.VerifyAccess();

      if (_childrenLoadedTcs == null)
      {
        _childrenLoadedTcs = new TaskCompletionSource<IReadOnlyList<ITreeNode>>();
        if (!HasDummyChild)
        {
          _childrenLoadedTcs.TrySetResult(new ITreeNode[0]);
        }
      }

      if (HasDummyChild)
      {
        IsBeingExpanded = true;
        Children.Remove(DummyChild);
        DoLoadChildrenAsync().ContinueWith(t =>
        {
          var items = t.Result;
          _dispatcher.BeginInvoke(() =>
          {
            Children.AddRange(items);
            IsBeingExpanded = false;
            _childrenLoadedTcs.TrySetResult(items);
          });
        });
      }

      return _childrenLoadedTcs.Task;
    }
    #endregion Public Methods

    #region Overrides
    public override bool HasDummyChild
    {
      get { return Children.Count == 1 && Children[0] == DummyChild; }
    }

    protected override void OnIsExpandedChanged()
    {
      LoadChildrenAsync().ConfigureAwait(true);
    }
    #endregion Overrides

    #region Private Methods
    private Task<IReadOnlyList<ITreeNode>> DoLoadChildrenAsync()
    {
      return TreeNodesOperation.CreateChildrenAsync(DataItem, this);
    }
    #endregion Private Methods
  }
}