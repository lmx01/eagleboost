// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-20 12:11 AM

namespace eagleboost.presentation.Controls.TreeView
{
  /// <summary>
  /// TreeNodeData
  /// </summary>
  public class TreeNodeData : TreeNode
  {
    #region ctors
    public TreeNodeData(object dataItem, ITreeNodeContainer parent, ITreeNodesOperation treeNodesOperation) : base(dataItem, parent, treeNodesOperation)
    {
    }
    #endregion ctors
  }
}