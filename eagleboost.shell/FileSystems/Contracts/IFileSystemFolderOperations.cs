// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 23 2:15 PM

namespace eagleboost.shell.FileSystems.Contracts
{
  using System.Windows.Input;
  using eagleboost.core.ComponentModel;

  /// <summary>
  /// IFileSystemFolderOperations
  /// </summary>
  public interface IFileSystemFolderOperations : IDisplayItem
  {
    #region Properties
    IFolder DriveFolder { get; }

    ICommand NavigateToCommand { get; }
    #endregion Properties
  }
}