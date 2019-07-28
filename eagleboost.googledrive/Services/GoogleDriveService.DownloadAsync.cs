// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 27 6:44 PM

namespace eagleboost.googledrive.Services
{
  using System;
  using System.IO;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.core.Logging;
  using eagleboost.core.Utils;
  using Google.Apis.Download;

  /// <summary>
  /// GoogleDriveService
  /// </summary>
  public partial class GoogleDriveService
  {
    public Task DownloadAsync(string id, Stream stream, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(() => DoDownloadAsync(id, stream, ct, progress), ct);
    }

    private async Task DoDownloadAsync(string id, Stream stream, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      ArgValidation.ThrowIfNull(stream, "stream");

      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);
      try
      {
        var request = driveService.Files.Get(id);
        request.SupportsTeamDrives = true;
        if (progress != null)
        {
          request.MediaDownloader.ProgressChanged += p =>
          {
            switch (p.Status)
            {
              case DownloadStatus.Downloading:
              {
                progress.Report("Bytes downloaded: " + p.BytesDownloaded);
                break;
              }
              case DownloadStatus.Completed:
              {
                progress.Report("Download completed");
                break;
              }
              case DownloadStatus.Failed:
              {
                progress.Report("Download failed");
                break;
              }
            }
          };
        }

        await request.DownloadAsync(stream, ct).ConfigureAwait(false);
        stream.Seek(0, SeekOrigin.Begin);
      }
      catch (Exception ex)
      {
        Logger.Error("Error deleting file {0} - {1}", id, ex);
      }
    }
  }
}