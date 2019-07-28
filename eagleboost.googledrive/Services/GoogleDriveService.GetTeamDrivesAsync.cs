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
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Models;

  /// <summary>
  /// GoogleDriveService
  /// </summary>
  public partial class GoogleDriveService
  {
    public Task<IReadOnlyCollection<IGoogleDriveFile>> GetTeamDrivesAsync(IGoogleDriveFolder parent, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(() => DoGetTeamDrivesAsync(parent, ct, progress), ct);
    }

    private async Task<IReadOnlyCollection<IGoogleDriveFile>> DoGetTeamDrivesAsync(IGoogleDriveFolder parent, CancellationToken ct, IProgress<string> progress)
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);

      var teamDrives = driveService.Teamdrives.List();
      var t = await teamDrives.ExecuteAsync(ct);
      var teamDriveFolders = t.TeamDrives.Select(i => new GoogleTeamDriveFolder(i, parent, _ => DoGetChildFilesAsync(_, null, ct, progress))).ToArray();
      return teamDriveFolders;
    }
  }
}