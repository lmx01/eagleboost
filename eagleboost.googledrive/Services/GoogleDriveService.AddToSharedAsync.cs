// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 27 6:44 PM

namespace eagleboost.googledrive.Services
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.core.Extensions;
  using eagleboost.core.Logging;
  using Google.Apis.Drive.v3.Data;

  /// <summary>
  /// GoogleDriveService
  /// </summary>
  public partial class GoogleDriveService
  {
    public Task AddToSharedAsync(string fileId, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(() => DoAddToSharedAsync(fileId, ct, progress), ct);
    }

    private async Task DoAddToSharedAsync(string fileId, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);
      var sharedFolder = await GetOrCreateFolderAsync("_Shared", null, ct);
      var sharedFolderId = sharedFolder.Folder.Id;
      var file = await DoGetGoogleDriveFileAsync(fileId, null, ct, progress);
      if (file.OwnedByMe)
      {
        Logger.Info(file.Name + "[" + fileId + "] is not shared by others");
        return;
      }

      if (file.Parents != null && file.Parents.Contains(sharedFolderId))
      {
        Logger.Info(file.Name + "[" + fileId + "] already exists in " + sharedFolder.Folder);
        return;
      }

      var updateRequest = driveService.Files.Update(new File(), fileId);
      updateRequest.Fields = "id, parents";
      updateRequest.AddParents = sharedFolder.Folder.Id;
      await updateRequest.ExecuteAsync(ct);
      var log = "Added " + file.Name + "[" + fileId + "]  to " + sharedFolder.Folder;
      progress.TryReport(log);
      Logger.Info(log);
    }
  }
}