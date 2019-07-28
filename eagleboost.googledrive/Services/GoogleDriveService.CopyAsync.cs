// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 27 6:44 PM

namespace eagleboost.googledrive.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.core.Extensions;
  using eagleboost.core.Logging;
  using eagleboost.core.Threading;
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Models;
  using eagleboost.googledrive.Types;
  using Google.Apis.Drive.v3.Data;

  /// <summary>
  /// GoogleDriveService
  /// </summary>
  public partial class GoogleDriveService
  {
    public Task<IGoogleDriveFile> CopyAsync(IGoogleDriveFile from, IGoogleDriveFolder toFolder, PauseToken pt = default(PauseToken), CancellationToken ct = default(CancellationToken), IProgress<GoogleDriveProgress> progress = null, GoogleDriveProgress progressPayload = null)
    {
      return Task.Run(async () =>
      {
        var payload = progressPayload ?? new GoogleDriveProgress();
        var result = await DoCopyAsync(from, toFolder, pt, ct, progress, payload).ConfigureAwait(false);
        progress.TryReport(() => payload);
        return result;
      }, ct);
    }

    public Task<IGoogleDriveFile> DoCopyAsync(IGoogleDriveFile from, IGoogleDriveFolder toFolder, PauseToken pt, CancellationToken ct, IProgress<GoogleDriveProgress> progress, GoogleDriveProgress progressPayload)
    {
      var folder = from as IGoogleDriveFolder;
      return folder != null
        ? DoCopyFolderAsync(folder, toFolder, pt, ct, progress, progressPayload)
        : DoCopyFileAsync(from, toFolder, pt, ct, progress, progressPayload);
    }

    private async Task<IGoogleDriveFile> DoCopyFileAsync(IGoogleDriveFile from, IGoogleDriveFolder toFolder, PauseToken pt, CancellationToken ct, IProgress<GoogleDriveProgress> progress, GoogleDriveProgress progressPayload)
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);
      progressPayload.Count++;
      progressPayload.Current = from;
      progressPayload.Status = string.Format("Copying file '{0}' to folder '{1}'...", from.Name, toFolder.Name);
      progress.TryReport(() => progressPayload);

      await pt.WaitWhilePausedAsync().ConfigureAwait(false);
      var toFile = new File { Parents = new List<string> { toFolder.Id } };
      var copyRequest = driveService.Files.Copy(toFile, from.Id);
      copyRequest.SupportsTeamDrives = true;
      var resp = await copyRequest.ExecuteAsync(ct).ConfigureAwait(false);
      var result = new GoogleDriveFile(resp, toFolder);
      RaiseFileCreated(result);
      return result;
    }

    private async Task<IGoogleDriveFile> DoCopyFolderAsync(IGoogleDriveFolder from, IGoogleDriveFolder toFolder, PauseToken pt, CancellationToken ct, IProgress<GoogleDriveProgress> progress, GoogleDriveProgress progressPayload)
    {
      await pt.WaitWhilePausedAsync().ConfigureAwait(false);
      var getOrCreateFolder = await GetOrCreateFolderAsync(from.Name, toFolder, ct, progress, progressPayload).ConfigureAwait(false);
      var fromFolderCopy = getOrCreateFolder.Folder;
      if (fromFolderCopy.IsChildrenCopied.GetValueOrDefault())
      {
        progressPayload.Status = string.Format("Folder '{0}' is already copied", fromFolderCopy.Name);
        progress.TryReport(() => progressPayload);
        return fromFolderCopy;
      }

      await pt.WaitWhilePausedAsync().ConfigureAwait(false);
      progressPayload.Status = string.Format("Loading files under source folder '{0}'", from.Name);
      progress.TryReport(() => progressPayload);
      var childFiles = await DoGetChildFilesAsync(from, null, ct, null).ConfigureAwait(false);

      IReadOnlyCollection<IGoogleDriveFile> childFilesOfCopy = null;
      if (!getOrCreateFolder.IsNew)
      {
        progressPayload.Status = string.Format("Loading files under target folder '{0}'", from.Name);
        progress.TryReport(() => progressPayload);
        childFilesOfCopy = await DoGetChildFilesAsync(fromFolderCopy, null, ct, null).ConfigureAwait(false);
      }

      var hasErrors = false;
      foreach (var file in childFiles)
      {
        await pt.WaitWhilePausedAsync().ConfigureAwait(false);
        if (!ct.IsCancellationRequested)
        {
          ////Check existing in memory here instead of letting DoCopyFileAsync to check the file existence by using name to check the target folder - that often throws 400 ApiException.
          var existing = childFilesOfCopy != null ? childFilesOfCopy.FirstOrDefault(i => i.Name == file.Name && i.Size == file.Size) : null;
          if (existing == null || file is IGoogleDriveFolder)
          {
            try
            {
              await DoCopyAsync(file, fromFolderCopy, pt, ct, progress, progressPayload).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
              Logger.Error("Error copy file {0} - {1}", from.Name, ex);
              hasErrors = true;
            }
          }
          else
          {
            progressPayload.Count++;
            progressPayload.Current = existing;
            progressPayload.Status = string.Format("Found file '{0}' in folder '{1}'...", file.Name, fromFolderCopy.Name);
            progress.TryReport(() => progressPayload);
          }
        }
      }

      if (!hasErrors)
      {
        await UpdateChildrenCopiedAsync(fromFolderCopy, ct).ConfigureAwait(false);
      }

      return fromFolderCopy;
    }

    private async Task UpdateChildrenCopiedAsync(IGoogleDriveFolder folder, CancellationToken ct)
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);
      try
      {
        var appProperties = new Dictionary<string, string> { { "ChildrenCopied", "True" } };
        var updateFolder = new File { MimeType = MimeType.Folder, AppProperties = appProperties };
        var updateRequest = driveService.Files.Update(updateFolder, folder.Id);
        updateRequest.SupportsTeamDrives = true;
        await updateRequest.ExecuteAsync(ct).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        Logger.Error("Fail to update AppProperties for folder {0} - {1}", folder, ex);
      }
    }
  }
}