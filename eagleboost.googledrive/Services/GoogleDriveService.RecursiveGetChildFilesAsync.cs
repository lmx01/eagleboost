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

  /// <summary>
  /// GoogleDriveService
  /// </summary>
  public partial class GoogleDriveService
  {
    public Task<IDictionary<IGoogleDriveFile, int>> RecursiveGetChildFilesAsync(IGoogleDriveFolder parent, int start, string filenameQuery = null, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(async () =>
      {
        var items = await DoRecursiveGetChildFilesAsync(parent, start, filenameQuery, ct, progress).ConfigureAwait(false);
        return (IDictionary<IGoogleDriveFile, int>)items.ToDictionary(i => i.Item1, i => i.Item2);
      }, ct);
    }

    private async Task<IReadOnlyCollection<Tuple<IGoogleDriveFile, int>>> DoRecursiveGetChildFilesAsync(IGoogleDriveFile file, int start, string filenameQuery, CancellationToken ct, IProgress<string> progress)
    {
      var folder = file as IGoogleDriveFolder;
      if (folder != null)
      {
        var result = new List<Tuple<IGoogleDriveFile, int>> { Tuple.Create(file, start++) };
        var q = string.Format(CommonQueries.LiveFileFormat, folder.Id);
        progress.TryReport("Recursively load files in " + folder + ", FileName: " + filenameQuery);
        var files = await DoRecursiveGetChildFilesAsync(folder, q, start, filenameQuery, ct, progress).ConfigureAwait(false);
        result.AddRange(files);
        return result;
      }

      return new[] { Tuple.Create(file, start) };
    }

    private async Task<IReadOnlyCollection<Tuple<IGoogleDriveFile, int>>> DoRecursiveGetChildFilesAsync(IGoogleDriveFolder parent, string query, int start, string filenameQuery, CancellationToken ct, IProgress<string> progress)
    {
      var newQuery = query;
      if (filenameQuery.HasValue())
      {
        var fileOrFolder = CommonQueries.IsFolder + " or ((" + filenameQuery + ") and " + CommonQueries.IsNotFolder + ")";
        newQuery = newQuery + " and (" + fileOrFolder + ")";
      }

      var files = await DoGetFilesAsync(newQuery, ct, progress).ConfigureAwait(false);
      var gFiles = files.Select(f => CreateDriveFile(f, parent, ct, null)).ToArray();

      var result = new List<Tuple<IGoogleDriveFile, int>>();
      foreach (var file in gFiles)
      {
        if (file is IGoogleDriveFolder)
        {
          var children = await DoRecursiveGetChildFilesAsync(file, start, filenameQuery, ct, progress).ConfigureAwait(false);
          start += children.Count;
          result.AddRange(children);
        }
        else
        {
          result.Add(Tuple.Create(file, start++));
        }
      }
      return result;
    }
  }
}