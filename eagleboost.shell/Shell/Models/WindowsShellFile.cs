// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-29 10:35 PM

namespace eagleboost.shell.Shell.Models
{
  using System;
  using eagleboost.shell.FileSystems.Models;
  using eagleboost.shell.Shell.Contracts;
  using Microsoft.WindowsAPICodePack.Shell;

  /// <summary>
  /// WindowsShellFile
  /// </summary>
  public class WindowsShellFile : FileBase<WindowsShellFile, IWindowsShellFolder>, IWindowsShellFile
  {
    #region Declarations
    private readonly ShellObject _file;
    #endregion Declarations

    #region ctors
    public WindowsShellFile(ShellObject file, IWindowsShellFolder parent) : base(parent)
    {
      _file = file;
    }
    #endregion ctors

    #region IWindowsShellFile
    public ShellObject ShellObject
    {
      get { return _file; }
    }
    #endregion IWindowsShellFile

    #region Overrides
    public override string Id
    {
      get { return _file.GetHashCode().ToString(); }
    }

    public override string Name
    {
      get { return _file.Name; }
    }

    public override string Type
    {
      get { return "File"; }
    }

    public override long? Size
    {
      get { return 0; }
    }

    public override DateTime? CreatedTime
    {
      get { return null; }
    }

    public override DateTime? ModifiedTime
    {
      get { return null; }
    }

    public override string LastModifyingUser
    {
      get { return null; }
    }
    #endregion Overrides
  }
}