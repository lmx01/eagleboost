// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-30 9:23 PM

namespace eagleboost.shell.FileSystems.Contracts
{
  using System.ComponentModel;
  using System.Threading.Tasks;
  using eagleboost.presentation.Collections;
  using eagleboost.presentation.Controls.TreeView;

  /// <summary>
  /// IFileSystemTreeViewModel
  /// </summary>
  public interface IFileSystemTreeViewModel : ICollectionViewModel<ITreeNode>, INotifyPropertyChanged, IFileSystemTreeNodesOperation
  {
    #region Properties
    ITreeNode Root { get; }
    #endregion Properties

    #region Methods
    Task<bool> SelectAsync(string[] path);
    #endregion Methods
  }
}