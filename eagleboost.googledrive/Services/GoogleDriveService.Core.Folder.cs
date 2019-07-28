// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 27 6:44 PM

namespace eagleboost.googledrive.Services
{
  using System;
  using System.Collections.Generic;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.core.Extensions;
  using eagleboost.core.Logging;
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Models;
  using eagleboost.googledrive.Types;
  using Google.Apis.Drive.v3.Data;

  /// <summary>
  /// GoogleDriveService
  /// </summary>
  public partial class GoogleDriveService
  {
    private async Task<GetOrCreateFolder> GetOrCreateFolderAsync(string name, IGoogleDriveFolder parent,
      CancellationToken ct, IProgress<GoogleDriveProgress> progress = null, GoogleDriveProgress progressPayload = null)
    {
      GetOrCreateFolder result;
      var folder = await GetFileAsync<IGoogleDriveFolder>(name, parent, ct, null).ConfigureAwait(false);
      if (folder != null)
      {
        result = new GetOrCreateFolder(folder, false);
        if (progressPayload != null)
        {
          progressPayload.Status = string.Format("Found folder '{0}' under {1}...", name, parent.Name);
        }
      }
      else
      {
        folder = await DoCreateFolderAsync(name, parent, ct, null).ConfigureAwait(false);
        result = new GetOrCreateFolder(folder, true);
        if (progressPayload != null)
        {
          progressPayload.Status = string.Format("Creating folder '{0}' under {1}...", name, parent.Name);
        }
      }

      if (progressPayload != null)
      {
        progressPayload.Count++;
        progressPayload.Current = folder;
        progress.TryReport(() => progressPayload);
      }

      return result;
    }

    private async Task<IGoogleDriveFolder> DoCreateFolderAsync(string name, IGoogleDriveFolder parent, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      progress.TryReport("Creating folder '{0}' under folder '{1}'...", name, parent.Name);
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);
      try
      {
        var folder = new File { Name = name, MimeType = MimeType.Folder, Parents = new List<string> { parent.Id } };
        var createRequest = driveService.Files.Create(folder);
        createRequest.SupportsTeamDrives = true;
        var resp = await createRequest.ExecuteAsync(ct).ConfigureAwait(false);
        var result = new GoogleDriveFolder(resp, parent, _ => DoGetChildFilesAsync(_, null, ct, progress));
        RaiseFileCreated(result);
        return result;
      }
      catch (Exception ex)
      {
        Logger.Error("Error creating folder {0} - {1}", name, ex);
        return null;
      }
    }

    /// <summary>
    /// GetOrCreateFolder
    /// </summary>
    private struct GetOrCreateFolder
    {
      public GetOrCreateFolder(IGoogleDriveFolder folder, bool isNew)
      {
        Folder = folder;
        IsNew = isNew;
      }

      public readonly IGoogleDriveFolder Folder;

      public readonly bool IsNew;
    }
  }
}