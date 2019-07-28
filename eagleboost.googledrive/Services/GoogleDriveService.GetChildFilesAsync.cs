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
  using eagleboost.googledrive.Contracts;
  using Google.Apis.Drive.v3.Data;

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
      var childFilesSubject = new Subject<IReadOnlyCollection<IGoogleDriveFile>>();

      Task.Run(() => QueryObservableChildFilesAsync(childFilesSubject, parent, query, ct, progress), ct);

      return childFilesSubject.ToTask(ct);
    }
  }
}