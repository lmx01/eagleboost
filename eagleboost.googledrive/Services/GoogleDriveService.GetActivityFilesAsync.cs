// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 27 6:44 PM

namespace eagleboost.googledrive.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reactive.Subjects;
  using System.Reactive.Threading.Tasks;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.core.Extensions;
  using eagleboost.core.Logging;
  using eagleboost.googledrive.Contracts;
  using Google.Apis.Drive.v3.Data;

  /// <summary>
  /// GoogleDriveService
  /// </summary>
  public partial class GoogleDriveService
  {
    public string StartPageToken { get; set; }

    public Task<IReadOnlyCollection<IGoogleDriveFile>> GetActivityFilesAsync(IGoogleDriveFolder parent, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(() => DoGetActivityFilesAsync(parent, ct, progress), ct);
    }

    private async Task<IReadOnlyCollection<IGoogleDriveFile>> DoGetActivityFilesAsync(IGoogleDriveFolder parent, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);

      var result = new List<IGoogleDriveFile>();
      var pageToken = await GetStartPageTokenAsync().ConfigureAwait(false);
      while (pageToken != null)
      {
        var request = driveService.Changes.List(pageToken);
        request.Spaces = "drive";
        request.IncludeTeamDriveItems = true;
        request.IncludeCorpusRemovals = true;
        request.IncludeRemoved = true;
        request.SupportsTeamDrives = true;
        var changes = await request.ExecuteAsync(ct).ConfigureAwait(false);
        var fileIds = changes.Changes.Where(c => c.File != null).Select(c => c.FileId).ToArray();
        var changedFiles = new List<IGoogleDriveFile>();
        foreach (var id in fileIds)
        {
          try
          {
            var file = await DoGetGoogleDriveFileAsync(id, parent, ct, progress).ConfigureAwait(false);
            if (!file.OwnedByMe)
            {
              changedFiles.Add(file);
            }
          }
          catch (Exception ex)
          {
            Logger.Info("Error loading file info " + id + " - " + ex);
          }
        }
        Logger.Info("Found " + changedFiles.Count + " file changes");
        result.AddRange(changedFiles);

        if (changes.NewStartPageToken != null)
        {
          // Last page, save this token for the next polling interval
          StartPageToken = changes.NewStartPageToken;
          break;
        }
        pageToken = changes.NextPageToken;
        break;
      }

      return result;
    }

    private async Task<string> GetStartPageTokenAsync()
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);
      var req = driveService.Changes.GetStartPageToken();
      var token = await req.ExecuteAsync();
      var tokenValue = int.Parse(token.StartPageTokenValue);
      return (tokenValue - 1000).ToString();
    }
  }
}