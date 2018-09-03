// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-19 11:53 PM

namespace eagleboost.presentation.Controls.TreeView
{
  using System.Collections.Generic;
  using System.Threading.Tasks;

  public interface ITreeNodesOperation
  {
    #region Methods
    Task<ITreeNode> InitializeAsync(bool isSingleRoot);

    Task<ITreeNode> CreateChildAsync(object childDataItem, ITreeNodeContainer parent);

    Task<IReadOnlyList<ITreeNode>> CreateChildrenAsync(object parentDataItem, ITreeNodeContainer parent);

    bool Filter(ITreeNode item);
    #endregion Methods    
  }
}