// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 27 6:44 PM

namespace eagleboost.googledrive.Services
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.googledrive.Contracts;

  /// <summary>
  /// GoogleDriveService
  /// </summary>
  public partial class GoogleDriveService
  {
    public Task<IGoogleDriveFile> GetFileAsync(string id, IGoogleDriveFolder parent, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(() => DoGetGoogleDriveFileAsync(id, parent, ct, progress), ct);
    }

    private async Task<IGoogleDriveFile> DoGetGoogleDriveFileAsync(string id, IGoogleDriveFolder parent, CancellationToken ct, IProgress<string> progress)
    {
      var f = await DoGetFileAsync(id, ct, progress).ConfigureAwait(false);
      var result = CreateDriveFile(f, parent, ct, progress);
      return result;
    }
  }
}