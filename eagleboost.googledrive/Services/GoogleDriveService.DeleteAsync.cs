// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 27 6:44 PM

namespace eagleboost.googledrive.Services
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.core.Logging;
  using Google.Apis.Drive.v3.Data;

  /// <summary>
  /// GoogleDriveService
  /// </summary>
  public partial class GoogleDriveService
  {
    public Task DeleteAsync(string id, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(() => DoDeleteAsync(id, ct, progress), ct);
    }

    private async Task DoDeleteAsync(string id, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);
      try
      {
        var deleteRequest = driveService.Files.Delete(id);
        deleteRequest.SupportsTeamDrives = true;
        await deleteRequest.ExecuteAsync(ct).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        Logger.Error("Error deleting file {0} - {1}", id, ex);
      }
    }

    public Task MoveToTrashAsync(string id, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(() => DoMoveToTrashAsync(id, ct, progress), ct);
    }

    private async Task DoMoveToTrashAsync(string id, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);
      try
      {
        var file = new File { Trashed = true, Id = id };
        var updateRequest = driveService.Files.Update(file, id);
        updateRequest.SupportsTeamDrives = true;
        await updateRequest.ExecuteAsync(ct).ConfigureAwait(false);
        Logger.Info("Moved to trash " + id);
      }
      catch (Exception ex)
      {
        Logger.Error("Error deleting file {0} - {1}", id, ex);
      }
    }
  }
}