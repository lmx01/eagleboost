// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-31 12:55 AM

namespace eagleboost.shell.Shell.Models
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.shell.FileSystems.Contracts;
  using eagleboost.shell.FileSystems.Extensions;
  using eagleboost.shell.FileSystems.Models;
  using eagleboost.shell.Shell.Contracts;
  using Microsoft.WindowsAPICodePack.Shell;

  /// <summary>
  /// WindowsShellFolder
  /// </summary>
  public class WindowsShellFolder :  FileBase<WindowsShellFolder, IWindowsShellFolder>, IWindowsShellFolder
  {
    #region Declarations
    private readonly Func<WindowsShellFolder, Task<IReadOnlyList<IWindowsShellFile>>> _filesFunc;
    private readonly ShellContainer _folder;
    #endregion Declarations

    #region ctors
    public WindowsShellFolder(ShellContainer folder, IWindowsShellFolder parent, Func<WindowsShellFolder, Task<IReadOnlyList<IWindowsShellFile>>> filesFunc) : base(parent)
    {
      _folder = folder;
      _filesFunc = filesFunc;
    }
    #endregion ctors

    #region IWindowsShellFolder
    public ShellObject ShellObject
    {
      get { return _folder; }
    }

    public async Task<IReadOnlyList<IFile>> GetFilesAsync(CancellationToken ct = default(CancellationToken))
    {
      return await _filesFunc(this).ConfigureAwait(false);
    }
    #endregion IWindowsShellFolder

    #region Overrides
    public override string Id
    {
      get { return _folder.ParsingName; }
    }

    public override string Name
    {
      get { return _folder.Name; }
    }

    public override string Type
    {
      get { return "Folder"; }
    }

    public override long? Size
    {
      get { return null; }
    }

    public override string ToString()
    {
      var levels = new List<string>(this.FolderToRoot<IFolder>().Select(i => i.Name).Reverse());
      var result = string.Join(" -> ", levels);
      return result;
    }
    #endregion Overrides
  }
}