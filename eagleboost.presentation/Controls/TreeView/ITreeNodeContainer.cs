// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-02 1:18 PM

namespace eagleboost.presentation.Controls.TreeView
{
  using System.Threading.Tasks;

  /// <summary>
  /// ITreeNodeContainer
  /// </summary>
  public interface ITreeNodeContainer : ITreeNode
  {
    #region Methods
    Task ExpandAsync();

    Task LoadChildrenAsync();

    Task RefreshAsync();

    void AddData(object dataItem);

    void RemoveData(object dataItem);

    void Clear();
    #endregion Methods 
  }
}