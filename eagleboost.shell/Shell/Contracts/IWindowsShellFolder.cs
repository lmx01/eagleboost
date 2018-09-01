// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-31 1:06 AM

namespace eagleboost.shell.Shell.Contracts
{
  using eagleboost.shell.FileSystems.Contracts;

  /// <summary>
  /// IWindowsShellFolder
  /// </summary>
  public interface IWindowsShellFolder : IFolder, IWindowsShellFile
  {
  }
}