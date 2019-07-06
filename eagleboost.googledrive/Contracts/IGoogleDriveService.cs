// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-21 9:18 PM

namespace eagleboost.googledrive.Contracts
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.core.Threading;
  using eagleboost.googledrive.Models;

  public interface IGoogleDriveService
  {
    #region Methods
    Task<IReadOnlyList<IGoogleDriveFile>> GetTeamDrivesAsync(IGoogleDriveFolder parent, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    Task<IReadOnlyList<IGoogleDriveFile>> GetChildFilesAsync(IGoogleDriveFolder parent, string query = null, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    Task<IDictionary<IGoogleDriveFile, int>> RecursiveGetChildFilesAsync(IGoogleDriveFolder parent, int start, CancellationToken ct = default(CancellationToken));

    Task<IReadOnlyList<IGoogleDriveFile>> GetFilesAsync(string query = null, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    Task<IGoogleDriveFile> GetFileAsync(string id, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    Task<IGoogleDriveFile> CopyAsync(IGoogleDriveFile from, IGoogleDriveFolder toFolder, PauseToken pt = default(PauseToken), CancellationToken ct = default(CancellationToken), IProgress<GoogleDriveProgress> progress = null, GoogleDriveProgress progressPayload = null);

    Task<IGoogleDriveFolder> CreateFolderAsync(string name, IGoogleDriveFolder parent, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    Task DeleteAsync(string id, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    Task DownloadAsync(string id, Stream stream, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);
    #endregion Methods

    #region Events
    event FileCreatedEventHandler FileCreated;
    #endregion Events
  }

  public class GoogldDriveFileCreatedEventArgs : EventArgs
  {
    public readonly IGoogleDriveFile File;

    public GoogldDriveFileCreatedEventArgs(IGoogleDriveFile file)
    {
      File = file;
    }
  }

  public delegate void FileCreatedEventHandler(object sender, GoogldDriveFileCreatedEventArgs args);
}