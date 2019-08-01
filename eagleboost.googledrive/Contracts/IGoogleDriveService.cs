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

  /// <summary>
  /// IGoogleDriveService
  /// </summary>
  public interface IGoogleDriveService
  {
    #region Properties
    string StartPageToken { get; set; }
    #endregion Properties

    #region Methods
    Task<IReadOnlyCollection<IGoogleDriveFile>> GetActivityFilesAsync(IGoogleDriveFolder parent, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    IObservable<IReadOnlyCollection<GoogleDriveActivity>> ObserveChanges(string startPageToken);

    IObservable<IReadOnlyCollection<GoogleDriveActivity>> ObserveActivities(CancellationToken ct = default(CancellationToken));

    IObservable<IReadOnlyCollection<IGoogleDriveFile>> GetChildFilesObservable(IGoogleDriveFolder parent, string query = null, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    Task<IReadOnlyCollection<IGoogleDriveFile>> GetTeamDrivesAsync(IGoogleDriveFolder parent, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    Task<IReadOnlyCollection<IGoogleDriveFile>> GetChildFilesAsync(IGoogleDriveFolder parent, string query = null, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    Task<IDictionary<IGoogleDriveFile, int>> RecursiveGetChildFilesAsync(IGoogleDriveFolder parent, int start, string filenameQuery = null, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    Task<IReadOnlyCollection<IGoogleDriveFile>> GetFilesAsync(string query = null, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    Task<IGoogleDriveFile> GetFileAsync(string id, IGoogleDriveFolder parent, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    Task<IGoogleDriveFile> CopyAsync(IGoogleDriveFile from, IGoogleDriveFolder toFolder, string newName = null, PauseToken pt = default(PauseToken), CancellationToken ct = default(CancellationToken), IProgress<GoogleDriveProgress> progress = null, GoogleDriveProgress progressPayload = null);

    Task<IGoogleDriveFolder> CreateFolderAsync(string name, IGoogleDriveFolder parent, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    Task DeleteAsync(string id, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    Task DownloadAsync(string id, Stream stream, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    Task AddToSharedAsync(string fileId, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);

    Task<IReadOnlyCollection<IGoogleDriveFile>> GetFullPathAsync(string fileId, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null);
    #endregion Methods

    #region Events
    event FileCreatedEventHandler FileCreated;
    #endregion Events
  }

  /// <summary>
  /// GoogldDriveFileCreatedEventArgs
  /// </summary>
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