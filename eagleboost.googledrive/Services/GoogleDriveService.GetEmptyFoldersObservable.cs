namespace eagleboost.googledrive.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reactive.Subjects;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.core.Extensions;
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Extensions;
  using eagleboost.googledrive.Types;

  /// <summary>
  /// GoogleDriveServiced
  /// </summary>
  public partial class GoogleDriveService
  {
    public IObservable<IReadOnlyCollection<IGoogleDriveFolder>> GetEmptyFoldersObservable(IGoogleDriveFolder parent, string query = null, CancellationToken ct = default(CancellationToken),
      IProgress<string> progress = null)
    {
      var childFilesSubject = new Subject<IReadOnlyCollection<IGoogleDriveFolder>>();

      Task.Run(() => QueryEmptyFoldersAsync(childFilesSubject, parent, query, ct, progress), ct);

      return childFilesSubject;
    }
    
    private async Task QueryEmptyFoldersAsync(IObserver<IReadOnlyCollection<IGoogleDriveFolder>> observer, IGoogleDriveFolder parent, string query, CancellationToken ct, IProgress<string> progress)
    {
      var resultSub = new Subject<IList<IGoogleDriveFolder>>();

      Action<IGoogleDriveFolder> handleNext = f => observer.OnNext(new[] {f});

      var sub = new Subject<IGoogleDriveFolder>();
      sub.Subscribe(handleNext, resultSub.OnError, resultSub.OnCompleted);
      await DoQueryObservableEmptyFoldersAsync(sub, parent, query, 0, ct, progress);
    }
  
    private async Task DoQueryObservableEmptyFoldersAsync(IObserver<IGoogleDriveFolder> observer, IGoogleDriveFolder parent, string query, int level, CancellationToken ct, IProgress<string> progress)
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);

      string pageToken = null;
      if (ct != default(CancellationToken))
      {
        ct.Register(() => pageToken = null);
      }

      try
      {
        do
        {
          var q = level == 0
            ? string.Format(CommonQueries.LiveFolderFormat, parent.Id)
            : string.Format(CommonQueries.LiveFileFormat, parent.Id);
          
          if (query.HasValue())
          {
            q = q + " and " + query;
          }

          var request = driveService.GetListRequest(q, pageToken);
          var resp = await request.ExecuteAsync(ct).ConfigureAwait(false);
          if (!ct.IsCancellationRequested)
          {
            var files = resp.Files;
            if (files != null && files.Count > 0)
            {
              var newLevel = level + 1;
              foreach (var f in files.Where(i => i.MimeType == MimeType.Folder))
              {
                var folder = (IGoogleDriveFolder) CreateDriveFile(f, parent, ct, progress);
                await DoQueryObservableEmptyFoldersAsync(observer, folder, query, newLevel, ct, progress);
              }
            }
            else
            {
              progress.TryReport("Found empty folder " + parent);
              observer.OnNext(parent);
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

      if (level == 0)
      {
        observer.OnCompleted();
      }
    }
  }
}