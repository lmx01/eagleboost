// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-30 3:51 PM

namespace eagleboost.shell.FileSystems.Extensions
{
  using System.Linq;
  using System.Threading.Tasks;
  using eagleboost.shell.FileSystems.Contracts;

  public static class FileSystemTreeViewModelExt
  {
    public static Task<bool> SelectAsync(this IFileSystemTreeViewModel viewModel, IFolder toSelect)
    {
      var path = toSelect.FolderToRoot<IFolder>().Reverse().Select(i => i.Name).ToArray();
      return viewModel.SelectAsync(path);
    }
  }
}