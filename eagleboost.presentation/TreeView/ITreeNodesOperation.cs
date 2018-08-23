// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-19 11:53 PM

namespace eagleboost.presentation.TreeView
{
  using System.Collections.Generic;
  using System.Threading.Tasks;

  public interface ITreeNodesOperation
  {
    #region Methods
    Task<IEnumerable<ITreeNode>> CreateChildrenAsync(object parentDataItem, TreeNodeContainer parent);

    bool Filter(ITreeNode item);
    #endregion Methods    
  }
}