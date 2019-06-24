// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 23 8:40 PM

namespace eagleboost.shell.FileSystems.Contracts
{
  using System.Windows.Input;

  /// <summary>
  /// IFileSystemHistoryOperations
  /// </summary>
  public interface IFileSystemHistoryOperations
  {
    #region Properties
    IFolder DriveFolder { get; }

    ICommand NavigateToCommand { get; }
    #endregion Properties
  }
}