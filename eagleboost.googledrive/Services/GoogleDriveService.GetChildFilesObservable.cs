// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 27 6:44 PM

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
  using Google.Apis.Drive.v3.Data;

  /// <summary>
  /// GoogleDriveService
  /// </summary>
  public partial class GoogleDriveService
  {
    public IObservable<IReadOnlyCollection<IGoogleDriveFile>> GetChildFilesObservable(IGoogleDriveFolder parent, string query = null, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      var childFilesSubject = new Subject<IReadOnlyCollection<IGoogleDriveFile>>();

      Task.Run(() => QueryObservableChildFilesAsync(childFilesSubject, parent, query, ct, progress), ct);

      return childFilesSubject;
    }

    private async Task QueryObservableChildFilesAsync(IObserver<IReadOnlyCollection<IGoogleDriveFile>> observer, IGoogleDriveFolder parent, string query, CancellationToken ct, IProgress<string> progress)
    {
      var q = string.Format(CommonQueries.LiveFileFormat, parent.Id);
      if (query.HasValue())
      {
        q = q + " and " + query;
      }

      Action<IList<File>> handleNext = lf =>
      {
        var gFiles = lf.Select(f => CreateDriveFile(f, parent, ct, progress)).ToArray();
        observer.OnNext(gFiles);
      };

      var sub = new Subject<IList<File>>();
      sub.Subscribe(handleNext, observer.OnError, observer.OnCompleted, ct);
      await QueryObservableFilesAsync(sub, q, ct, progress);
    }
  }
}