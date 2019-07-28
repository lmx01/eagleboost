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
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Extensions;
  using eagleboost.googledrive.Models;
  using Google.Apis.Drive.v3.Data;

  /// <summary>
  /// GoogleDriveService
  /// </summary>
  public partial class GoogleDriveService
  {
    private IGoogleDriveFile CreateDriveFile(File f, IGoogleDriveFolder parent, CancellationToken ct, IProgress<string> progress)
    {
      return f.IsFolder() ? (IGoogleDriveFile)new GoogleDriveFolder(f, parent, _ => DoGetChildFilesAsync(_, null, ct, progress)) : new GoogleDriveFile(f, parent);
    }

    private async Task<File> DoGetFileAsync(string id, CancellationToken ct, IProgress<string> progress)
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);
      var request = driveService.GetFileRequest(id);
      var file = await request.ExecuteAsync(ct);
      progress.TryReport("Loaded file ", file.Name + "[" + id + "]");
      return file;
    }

    private async Task<T> GetFileAsync<T>(string name, IGoogleDriveFolder toFolder, CancellationToken ct, IProgress<string> progress) where T : class, IGoogleDriveFile
    {
      var query = string.Format(CommonQueries.LiveFileFormat + " and name = '{1}'", toFolder.Id, NormalizeName(name));
      var files = await DoGetGoogleDriveFilesAsync(toFolder, query, ct, progress).ConfigureAwait(false);
      if (files.Count == 1)
      {
        return files.First() as T;
      }

      if (files.Count > 1)
      {
        throw new ArgumentException(string.Format("More than one files found in folder '{0}', name: {1}", toFolder, name));
      }

      return null;
    }

    private async Task<IReadOnlyCollection<IGoogleDriveFile>> DoGetGoogleDriveFilesAsync(IGoogleDriveFolder parent, string query, CancellationToken ct, IProgress<string> progress)
    {
      var files = await DoGetFilesAsync(query, ct, progress).ConfigureAwait(false);
      var result = files.Select(f => CreateDriveFile(f, parent, ct, progress)).ToArray();
      return result;
    }

    private async Task<IReadOnlyCollection<File>> DoGetFilesAsync(string query, CancellationToken ct, IProgress<string> progress)
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);

      var total = 0;
      var result = new List<File>();
      string pageToken = null;
      do
      {
        var request = driveService.GetListRequest(query, pageToken);
        var resp = await request.ExecuteAsync(ct).ConfigureAwait(false);
        var files = resp.Files;
        if (files != null && files.Count > 0)
        {
          result.AddRange(files);
          total += files.Count;
          progress.TryReport("Loaded {0} files", total.ToString());
        }

        pageToken = resp.NextPageToken;
      }
      while (pageToken != null && !ct.IsCancellationRequested);

      return result;
    }

    private string NormalizeName(string name)
    {
      return name.Replace("'", "\\'");
    }

    private async Task QueryObservableFilesAsync(IObserver<IList<File>> observer, string query, CancellationToken ct, IProgress<string> progress)
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);

      var total = 0;
      string pageToken = null;
      if (ct != default(CancellationToken))
      {
        ct.Register(() => pageToken = null);
      }

      try
      {
        do
        {
          var request = driveService.GetListRequest(query, pageToken);
          var resp = await request.ExecuteAsync(ct).ConfigureAwait(false);
          if (!ct.IsCancellationRequested)
          {
            var files = resp.Files;
            if (files != null && files.Count > 0)
            {
              observer.OnNext(files);
              total += files.Count;
              progress.TryReport("Loaded {0} files", total.ToString());
            }

            pageToken = resp.NextPageToken;
          }

        } while (pageToken != null && !ct.IsCancellationRequested);
      }
      catch (Exception ex)
      {
        observer.OnError(ex);
        return;
      }

      observer.OnCompleted();
    }
  }
}