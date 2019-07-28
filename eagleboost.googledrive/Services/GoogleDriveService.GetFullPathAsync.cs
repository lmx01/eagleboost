// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 27 6:44 PM

namespace eagleboost.googledrive.Services
{
  using System;
  using System.Collections.Generic;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.core.Collections;
  using eagleboost.googledrive.Contracts;
  using Google.Apis.Drive.v3.Data;

  /// <summary>
  /// GoogleDriveService
  /// </summary>
  public partial class GoogleDriveService
  {
    public Task<IReadOnlyCollection<IGoogleDriveFile>> GetFullPathAsync(string fileId, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(() => DoGetFullPathAsync(fileId, ct, progress), ct);
    }

    private async Task<IReadOnlyCollection<IGoogleDriveFile>> DoGetFullPathAsync(string fileId, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      var fileStack = new Stack<File>();
      await DoGetFullPathRecursiveAsync(fileId, fileStack, ct, progress).ConfigureAwait(false);

      if (fileStack.Count == 1)
      {
        return Array<IGoogleDriveFile>.Empty;
      }

      var rootPath = fileStack.Pop();
      var parent = CreateDriveFile(rootPath, null, ct, progress);

      var googleFiles = new List<IGoogleDriveFile> { parent };
      foreach (var file in fileStack)
      {
        var googleFile = CreateDriveFile(file, parent as IGoogleDriveFolder, ct, progress);
        googleFiles.Add(googleFile);
      }

      return googleFiles;
    }

    private async Task DoGetFullPathRecursiveAsync(string fileId, Stack<File> fileStack, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      var file = await DoGetFileAsync(fileId, ct, progress).ConfigureAwait(false);
      if (file != null)
      {
        fileStack.Push(file);
        var parents = file.Parents;
        if (parents != null)
        {
          var parent = parents.Count == 1
            ? await DoGetFileAsync(parents[0], ct, progress).ConfigureAwait(false)
            : await DoGetParentOwnedByMeAsync(parents, ct, progress).ConfigureAwait(false);
          if (parent != null)
          {
            await DoGetFullPathRecursiveAsync(parent.Id, fileStack, ct, progress).ConfigureAwait(false);
          }
        }
      }
    }

    private async Task<File> DoGetParentOwnedByMeAsync(IList<string> parentIds, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      foreach (var p in parentIds)
      {
        var parent = await DoGetFileIfOwnedByMeAsync(p, ct, progress).ConfigureAwait(false);
        if (parent != null)
        {
          return parent;
        }
      }

      return null;
    }

    private async Task<File> DoGetFileIfOwnedByMeAsync(string id, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      var file = await DoGetFileAsync(id, ct, progress).ConfigureAwait(false);
      return file.OwnedByMe.GetValueOrDefault(false) ? file : null;
    }
  }
}