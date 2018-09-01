// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-31 1:04 AM

namespace eagleboost.shell.Shell.Models
{
  using Microsoft.WindowsAPICodePack.Shell;

  public sealed class WindowsShellRoot : ShellFolder
  {
    #region ctors
    public WindowsShellRoot()
    {
      Name = "Dummy Root";
    }
    #endregion ctors
  }
}