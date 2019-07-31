// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 27 6:44 PM

namespace eagleboost.googledrive.Services
{
  using System;
  using System.Collections.Generic;
  using System.Reactive.Subjects;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.googledrive.Contracts;

  /// <summary>
  /// GoogleDriveService
  /// </summary>
  public partial class GoogleDriveService
  {
    public Task<IReadOnlyCollection<IGoogleDriveFile>> GetChildFilesAsync(IGoogleDriveFolder parent, string query = null, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(() => DoGetChildFilesAsync(parent, query, ct, progress), ct);
    }

    private Task<IReadOnlyCollection<IGoogleDriveFile>> DoGetChildFilesAsync(IGoogleDriveFolder parent, string query, CancellationToken ct, IProgress<string> progress)
    {
      var tcs = new TaskCompletionSource<IReadOnlyCollection<IGoogleDriveFile>>();

      Task.Run(() =>
      {
        var result = new List<IGoogleDriveFile>();
        var observer = new Subject<IReadOnlyCollection<IGoogleDriveFile>>();
        observer.Subscribe(files => result.AddRange(files), () => tcs.TrySetResult(result));
        return QueryObservableChildFilesAsync(observer, parent, query, ct, progress);
      }, ct);

      return tcs.Task;
    }
  }
}