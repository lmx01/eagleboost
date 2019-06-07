// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-30 9:05 PM

namespace eagleboost.shell.FileSystems.Contracts
{
  using System.Collections.Generic;
  using System.Threading;
  using System.Threading.Tasks;

  /// <summary>
  /// IFolder
  /// </summary>
  public interface IFolder : IFile
  {
    #region Methods
    Task<IReadOnlyList<IFile>> GetFilesAsync(CancellationToken ct = default(CancellationToken));
    #endregion Methods
  }
}