// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-19 11:53 PM

namespace eagleboost.presentation.Controls.TreeView
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Threading.Tasks;

  public class TreeNodeContainer : TreeNode
  {
    private TaskCompletionSource<IEnumerable<ITreeNode>> _childrenLoadedTcs;

    public TreeNodeContainer(object shellObj, TreeNodeContainer parent, ITreeNodesOperation shellItemsOperation) : base(shellObj, parent, shellItemsOperation)
    {
      Children.Add(DummyChild);
    }

    public override bool HasDummyChild
    {
      get { return Children.Count == 1 && Children[0] == DummyChild; }
    }

    protected override async void OnIsExpandedChanged()
    {
      if (HasDummyChild)
      {
        Children.Remove(DummyChild);
        var items = await LoadChildrenAsync().ConfigureAwait(true);
        Children.AddRange(items);
      }
    }

    public override void Refresh()
    {
      {
        ChildrenView.Refresh();
      }
    }

    private Task<IEnumerable<ITreeNode>> LoadChildrenAsync()
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
  }
}