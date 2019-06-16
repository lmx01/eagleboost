// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 15 5:43 PM

namespace eagleboost.shell.FileSystems.Contracts
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;

  public interface IFileSystemFileInfoViewModel<in TFile, TFolder>
    where TFile : class, IFile
    where TFolder : IFolder
  {
    #region Methods
    Task LoadFileInfoAsync(TFile file, CancellationToken ct = default(CancellationToken), IProgress<string> process = null);
    #endregion Methods
  }
}