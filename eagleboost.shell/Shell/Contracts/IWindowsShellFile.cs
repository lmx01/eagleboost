// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-31 1:06 AM

namespace eagleboost.shell.Shell.Contracts
{
  using eagleboost.shell.FileSystems.Contracts;
  using Microsoft.WindowsAPICodePack.Shell;

  public interface IWindowsShellFile : IFile
  {
    ShellObject ShellObject { get; }
  }
}