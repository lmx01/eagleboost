// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-31 12:41 AM

namespace eagleboost.shell.Shell.ViewModels
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using eagleboost.core.Extensions;
  using eagleboost.presentation.Controls.TreeView;
  using eagleboost.shell.FileSystems.ViewModels;
  using eagleboost.shell.Shell.Contracts;
  using eagleboost.shell.Shell.Models;
  using Microsoft.WindowsAPICodePack.Shell;

  /// <summary>
  /// WindowsShellTreeViewModel
  /// </summary>
  public class WindowsShellTreeViewModel : FileSystemTreeViewModel
  {
    #region Statics
    private static readonly IKnownFolder DesktopKnownFolder = KnownFolders.Desktop;
    #endregion Statics

    #region Declarations
    private IWindowsShellFolder _rootFolder;
    private readonly WindowsShellRoot _shellRoot = new WindowsShellRoot();
    #endregion Declarations

    #region Overrides
    protected override bool DoFilter(ITreeNode node)
    {
      return node is DummyTreeNode || node.DataItem.CastTo<IWindowsShellFile>().ShellObject.IsNot<ShellFile>();
    }

    protected override TreeNodeContainer CreateRootNode()
    {
      _rootFolder = new WindowsShellFolder(_shellRoot, null, f => GetFilesAsync(f, DesktopKnownFolder));
      return new TreeNodeContainer(_rootFolder, null, this);
    }
    #endregion Overrides

    #region Private Methods
    private Task<IReadOnlyList<IWindowsShellFile>> GetFilesAsync(WindowsShellFolder parent, IEnumerable<ShellObject> shellObjects)
    {
      var result = new List<IWindowsShellFile>();
      ////shellObjects can't be enumerated in Task.Run, otherwise Drives would be lost.
      foreach (var shellObj in shellObjects)
      {
        var container = shellObj as IEnumerable<ShellObject>;
        var item = container != null
          ? (IWindowsShellFile)new WindowsShellFolder((ShellContainer)shellObj, parent, f => GetFilesAsync(f, container))
          : new WindowsShellFile(shellObj, parent);
        result.Add(item);
      }

      return Task.FromResult((IReadOnlyList<IWindowsShellFile>)result);
    }
    #endregion Private Methods
  }
}