// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-19 11:53 PM

namespace eagleboost.presentation.Controls.TreeView
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Threading.Tasks;

  /// <summary>
  /// TreeNodeContainer
  /// </summary>
  public class TreeNodeContainer : TreeNode
  {
    #region Declarations
    private TaskCompletionSource<IEnumerable<ITreeNode>> _childrenLoadedTcs;
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

    public async Task LoadChildrenAsync()
    {
      if (HasDummyChild)
      {
        IsBeingExpanded = true;
        try
        {
          Children.Remove(DummyChild);
          var items = await DoLoadChildrenAsync().ConfigureAwait(true);
          Children.AddRange(items);
        }
        finally
        {
          IsBeingExpanded = false;
        }
      }
    }
    #endregion Public Methods

    #region Overrides
    public override bool HasDummyChild
    {
      get { return Children.Count == 1 && Children[0] == DummyChild; }
    }

    protected override void OnIsExpandedChanged()
    {
      LoadChildrenAsync().ConfigureAwait(false);
    }
    #endregion Overrides

    #region Private Methods
    private Task<IEnumerable<ITreeNode>> DoLoadChildrenAsync()
    {
      if (_childrenLoadedTcs == null)
      {
        _childrenLoadedTcs = new TaskCompletionSource<IEnumerable<ITreeNode>>();
        Task.Run(async () =>
        {
          var items = await TreeNodesOperation.CreateChildrenAsync(DataItem, this).ConfigureAwait(true);
          _childrenLoadedTcs.TrySetResult(items);
        });
      }

      return _childrenLoadedTcs.Task;
    }
    #endregion Private Methods
  }
}