// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-19 11:53 PM

namespace eagleboost.presentation.Controls.TreeView
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Threading.Tasks;
  using System.Windows.Threading;
  using eagleboost.core.Collections;
  using eagleboost.presentation.Extensions;

  /// <summary>
  /// TreeNodeContainer
  /// </summary>
  public class TreeNodeContainer : TreeNode, ITreeNodeContainer
  {
    #region Declarations
    private TaskCompletionSource<IReadOnlyCollection<ITreeNode>> _childrenTcs = new TaskCompletionSource<IReadOnlyCollection<ITreeNode>>();
    private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;
    #endregion Declarations

    #region ctors
    public TreeNodeContainer(object dataItem, ITreeNodeContainer parent, ITreeNodesOperation shellItemsOperation) : base(dataItem, parent, shellItemsOperation)
    {
      Children.Add(DummyChild);
    }
    #endregion ctors

    #region ITreeNodeContainer
    public Task<IReadOnlyCollection<ITreeNode>> ChildrenTask
    {
      get { return _childrenTcs.Task; }
    }

    public async Task RefreshAsync()
    {
      if (IsExpanded || !HasDummyChild)
      {
        IsExpanded = false;
        _childrenTcs = null;
        Children.Clear();
        Children.Add(DummyChild);
        await ExpandAsync();
        if (HasDummyChild)
        {
          IsExpanded = false;
        }
      }
    }

    public async void AddData(object dataItem)
    {
      var node = await TreeNodesOperation.CreateChildAsync(dataItem, this).ConfigureAwait(true);
      Children.Add(node);
    }

    public void RemoveData(object dataItem)
    {
      var node = Children.SingleOrDefault(i => i.DataItem == dataItem);
      if (node != null)
      {
        Children.Remove(node);
      }
    }

    public void Clear()
    {
      Children.Clear();
    }
    #endregion ITreeNodeContainer

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

      if (_childrenTcs == null)
      {
        _childrenTcs = new TaskCompletionSource<IReadOnlyCollection<ITreeNode>>();
        if (!HasDummyChild)
        {
          _childrenTcs.TrySetResult(Array<ITreeNode>.Empty);
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
            _childrenTcs.TrySetResult(items);
          });
        });
      }

      return _childrenTcs.Task;
    }
    #endregion Public Methods

    #region Overrides
    public override bool HasDummyChild
    {
      get { return Children.Count >= 1 && Children[0] == DummyChild; }
    }

    protected override void OnIsExpandedChanged()
    {
      LoadChildrenAsync().ConfigureAwait(true);
    }
    #endregion Overrides

    #region Private Methods
    private Task<IReadOnlyCollection<ITreeNode>> DoLoadChildrenAsync()
    {
      return TreeNodesOperation.CreateChildrenAsync(DataItem, this);
    }
    #endregion Private Methods
  }
}