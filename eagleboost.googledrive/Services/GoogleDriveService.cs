// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-21 9:14 PM

namespace eagleboost.googledrive.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.core.Extensions;
  using eagleboost.core.Logging;
  using eagleboost.core.Threading;
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Extensions;
  using eagleboost.googledrive.Models;
  using eagleboost.googledrive.Types;
  using Google.Apis.Drive.v3;
  using Google.Apis.Drive.v3.Data;
  using Google.Apis.Services;

  /// <summary>
  /// GoogleDriveService
  /// </summary>
  public partial class GoogleDriveService : IGoogleDriveService
  {
    #region Declarations
    private readonly string _credentialsFile;
    private readonly string _applicationName;
    private readonly string _credentialTokenFile;
    private TaskCompletionSource<DriveService> _tcs;
    private readonly object _tcsLock = new object();
    private readonly ILoggerFacadeEx _logger;
    #endregion Declarations

    #region ctors
    public GoogleDriveService(string credentialsFile, string credentialTokenFile, string applicationName) : this(null, credentialsFile, credentialTokenFile, applicationName)
    {
    }

    public GoogleDriveService(ILoggerFacadeEx logger, string credentialsFile, string credentialTokenFile, string applicationName)
    {
      _logger = logger ?? LoggerManager.GetLogger<GoogleDriveService>();
      _credentialsFile = credentialsFile;
      _credentialTokenFile = credentialTokenFile;
      _applicationName = applicationName;
    }
    #endregion ctors

    #region Private Properties
    private ILoggerFacadeEx Logger
    {
      get { return _logger; }
    }
    #endregion Private Properties

    #region IGoogleDriveService
    public Task<IReadOnlyList<IGoogleDriveFile>> GetTeamDrivesAsync(IGoogleDriveFolder parent, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(() => DoGetTeamDrivesAsync(parent, ct, progress), ct);
    }

    public Task<IReadOnlyList<IGoogleDriveFile>> GetChildFilesAsync(IGoogleDriveFolder parent, string query = null, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(() => DoGetChildFilesAsync(parent, query, ct, progress), ct);
    }

    public Task<IDictionary<IGoogleDriveFile, int>> RecursiveGetChildFilesAsync(IGoogleDriveFolder parent, int start, CancellationToken ct = default(CancellationToken))
    {
      return Task.Run(async () =>
      {
        var items = await DoRecursiveGetChildFilesAsync(parent, start, ct).ConfigureAwait(false);
        return (IDictionary<IGoogleDriveFile, int>)items.ToDictionary(i => i.Item1, i => i.Item2);
      }, ct);
    }

    public Task<IReadOnlyList<IGoogleDriveFile>> GetFilesAsync(string query, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(() => DoGetGoogleDriveFilesAsync(null, query, ct, progress), ct);
    }

    public Task<IGoogleDriveFile> CopyAsync(IGoogleDriveFile from, IGoogleDriveFolder toFolder, PauseToken pt = default(PauseToken), CancellationToken ct = default(CancellationToken), IProgress<GoogleDriveProgress> progress = null, GoogleDriveProgress progressPayload = null)
    {
      return Task.Run(async() =>
      {
        var payload = progressPayload ?? new GoogleDriveProgress();
        var result = await DoCopyAsync(from, toFolder, pt, ct, progress, payload).ConfigureAwait(false);
        progress.TryReport(() => payload);
        return result;
      }, ct);
    }

    public Task<IGoogleDriveFolder> CreateFolderAsync(string name, IGoogleDriveFolder parent, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(() => DoCreateFolderAsync(name, parent, ct, progress), ct);
    }

    public Task DeleteAsync(string id, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(() => DoDeleteAsync(id, ct, progress), ct);
    }

    public event FileCreatedEventHandler FileCreated;

    protected virtual void RaiseFileCreated(IGoogleDriveFile file)
    {
      var handler = FileCreated;
      if (handler != null)
      {
        handler(this, new GoogldDriveFileCreatedEventArgs(file));
      }
    }
    #endregion IGoogleDriveService

    #region Private Methods
    private Task<DriveService> GetDriveServiceAsync()
    {
      lock (_tcsLock)
      {
        if (_tcs == null)
        {
          _tcs = new TaskCompletionSource<DriveService>();
          Task.Run(async () =>
          {
            var provider = new UserCredentialProvider();
            var credential = await provider.GetUserCredentialAsync(_credentialsFile, _credentialTokenFile).ConfigureAwait(true);
            var driveService = new DriveService(new BaseClientService.Initializer
            {
              HttpClientInitializer = credential,
              ApplicationName = _applicationName,
            });

            _tcs.TrySetResult(driveService);
          });
        }

        return _tcs.Task;
      }
    }

    private IGoogleDriveFile CreateDriveFile(File f, IGoogleDriveFolder parent, CancellationToken ct, IProgress<string> progress)
    {
      return f.IsFolder() ? (IGoogleDriveFile) new GoogleDriveFolder(f, parent, _ => DoGetChildFilesAsync(_, null, ct, progress)) : new GoogleDriveFile(f, parent);
    }

    private async Task<IReadOnlyList<IGoogleDriveFile>> DoGetTeamDrivesAsync(IGoogleDriveFolder parent, CancellationToken ct, IProgress<string> progress)
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);

      var teamDrives = driveService.Teamdrives.List();
      var t = await teamDrives.ExecuteAsync(ct);
      var teamDriveFolders = t.TeamDrives.Select(i => new GoogleTeamDriveFolder(i, parent, _ => DoGetChildFilesAsync(_, null, ct, progress))).ToArray();
      return teamDriveFolders;
    }

    private Task<IReadOnlyList<IGoogleDriveFile>> DoGetChildFilesAsync(IGoogleDriveFolder parent, string query, CancellationToken ct, IProgress<string> progress)
    {
      var q = string.Format(CommonQueries.LiveFileFormat, parent.Id);
      if (query.HasValue())
      {
        q = q + " and " + query;
      }

      return DoGetGoogleDriveFilesAsync(parent, q, ct, progress);
    }

    private async Task<IReadOnlyCollection<Tuple<IGoogleDriveFile, int>>> DoRecursiveGetChildFilesAsync(IGoogleDriveFile file, int start, CancellationToken ct)
    {
      var folder = file as IGoogleDriveFolder;
      if (folder != null)
      {
        var result = new List<Tuple<IGoogleDriveFile, int>> {Tuple.Create(file, start++)};
        var q = string.Format(CommonQueries.LiveFileFormat, folder.Id);
        var files = await DoRecursiveGetChildFilesAsync(folder, q, start, ct).ConfigureAwait(false);
        result.AddRange(files);
        return result;
      }

      return new[] { Tuple.Create(file, start) };
    }

    private async Task<IReadOnlyCollection<Tuple<IGoogleDriveFile, int>>> DoRecursiveGetChildFilesAsync(IGoogleDriveFolder parent, string query, int start, CancellationToken ct)
    {
      var files = await DoGetFilesAsync(query, ct, null).ConfigureAwait(false);
      var gFiles = files.Select(f => CreateDriveFile(f, parent, ct, null)).ToArray();

      var result = new List<Tuple<IGoogleDriveFile, int>>();
      foreach (var file in gFiles)
      {
        var children = await DoRecursiveGetChildFilesAsync(file, start, ct).ConfigureAwait(false);
        start += children.Count;
        result.AddRange(children);
      }
      return result;
    }

    private async Task<IReadOnlyList<IGoogleDriveFile>> DoGetGoogleDriveFilesAsync(IGoogleDriveFolder parent, string query, CancellationToken ct, IProgress<string> progress)
    {
      var files = await DoGetFilesAsync(query, ct, progress).ConfigureAwait(false);
      var result = files.Select(f => CreateDriveFile(f, parent, ct, progress)).ToArray();
      return result;
    }

    private async Task<IReadOnlyList<File>> DoGetFilesAsync(string query, CancellationToken ct, IProgress<string> progress)
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

    public Task<IGoogleDriveFile> DoCopyAsync(IGoogleDriveFile from, IGoogleDriveFolder toFolder, PauseToken pt, CancellationToken ct, IProgress<GoogleDriveProgress> progress, GoogleDriveProgress progressPayload)
    {
      var folder = from as IGoogleDriveFolder;
      return folder != null
        ? DoCopyFolderAsync(folder, toFolder, pt, ct, progress, progressPayload)
        : DoCopyFileAsync(from, toFolder, pt, ct, progress, progressPayload);
    }

    private async Task<IGoogleDriveFile> DoCopyFileAsync(IGoogleDriveFile from, IGoogleDriveFolder toFolder, PauseToken pt, CancellationToken ct, IProgress<GoogleDriveProgress> progress, GoogleDriveProgress progressPayload)
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);
      progressPayload.Count++;
      progressPayload.Current = from;
      progressPayload.Status = string.Format("Copying file '{0}' to folder '{1}'...", from.Name, toFolder.Name);
      progress.TryReport(() => progressPayload);

      await pt.WaitWhilePausedAsync().ConfigureAwait(false);
      var toFile = new File { Parents = new List<string> { toFolder.Id } };
      var copyRequest = driveService.Files.Copy(toFile, from.Id);
      copyRequest.SupportsTeamDrives = true;
      var resp = await copyRequest.ExecuteAsync(ct).ConfigureAwait(false);
      var result = new GoogleDriveFile(resp, toFolder);
      RaiseFileCreated(result);
      return result;
    }

    private async Task<IGoogleDriveFile> DoCopyFolderAsync(IGoogleDriveFolder from, IGoogleDriveFolder toFolder, PauseToken pt, CancellationToken ct, IProgress<GoogleDriveProgress> progress, GoogleDriveProgress progressPayload)
    {
      await pt.WaitWhilePausedAsync().ConfigureAwait(false);
      var getOrCreateFolder = await GetOrCreateFolderAsync(from, toFolder, ct, progress, progressPayload).ConfigureAwait(false);
      var fromFolderCopy = getOrCreateFolder.Folder;
      if (fromFolderCopy.IsChildrenCopied.GetValueOrDefault())
      {
        progressPayload.Status = string.Format("Folder '{0}' is already copied", fromFolderCopy.Name);
        progress.TryReport(() => progressPayload);
        return fromFolderCopy;
      }

      await pt.WaitWhilePausedAsync().ConfigureAwait(false);
      progressPayload.Status = string.Format("Loading files under source folder '{0}'", from.Name);
      progress.TryReport(() => progressPayload);
      var childFiles = await DoGetChildFilesAsync(from, null, ct, null).ConfigureAwait(false);

      IReadOnlyList<IGoogleDriveFile> childFilesOfCopy = null;
      if (!getOrCreateFolder.IsNew)
      {
        progressPayload.Status = string.Format("Loading files under target folder '{0}'", from.Name);
        progress.TryReport(() => progressPayload);
        childFilesOfCopy = await DoGetChildFilesAsync(fromFolderCopy, null, ct, null).ConfigureAwait(false);
      }

      var hasErrors = false;
      foreach (var file in childFiles)
      {
        await pt.WaitWhilePausedAsync().ConfigureAwait(false);
        if (!ct.IsCancellationRequested)
        {
          ////Check existing in memory here instead of letting DoCopyFileAsync to check the file existence by using name to check the target folder - that often throws 400 ApiException.
          var existing = childFilesOfCopy != null ? childFilesOfCopy.FirstOrDefault(i => i.Name == file.Name && i.Size == file.Size) : null;
          if (existing == null || file is IGoogleDriveFolder)
          {
            try
            {
              await DoCopyAsync(file, fromFolderCopy, pt, ct, progress, progressPayload).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
              Logger.Error("Error copy file {0} - {1}", from.Name, ex);
              hasErrors = true;
            }
          }
          else
          {
            progressPayload.Count++;
            progressPayload.Current = existing;
            progressPayload.Status = string.Format("Found file '{0}' in folder '{1}'...", file.Name, fromFolderCopy.Name);
            progress.TryReport(() => progressPayload);
          }
        }
      }

      if (!hasErrors)
      {
        await UpdateChildrenCopiedAsync(fromFolderCopy,ct).ConfigureAwait(false);
      }

      return fromFolderCopy;
    }

    private async Task UpdateChildrenCopiedAsync(IGoogleDriveFolder folder, CancellationToken ct)
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);
      try
      {
        var appProperties = new Dictionary<string, string> { { "ChildrenCopied", "True" } };
        var updateFolder = new File { MimeType = MimeType.Folder, AppProperties = appProperties };
        var updateRequest = driveService.Files.Update(updateFolder, folder.Id);
        updateRequest.SupportsTeamDrives = true;
        await updateRequest.ExecuteAsync(ct).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        Logger.Error("Fail to update AppProperties for folder {0} - {1}", folder, ex);
      }
    }

    private async Task<GetOrCreateFolder> GetOrCreateFolderAsync(IGoogleDriveFolder from, IGoogleDriveFolder toFolder,
      CancellationToken ct, IProgress<GoogleDriveProgress> progress, GoogleDriveProgress progressPayload)
    {
      GetOrCreateFolder result;
      var folder = await GetFileAsync<IGoogleDriveFolder>(from.Name, toFolder, ct, null).ConfigureAwait(false);
      if (folder != null)
      {
        result = new GetOrCreateFolder(folder, false);
        progressPayload.Status = string.Format("Found folder '{0}' under {1}...", from.Name, toFolder.Name);
      }
      else
      {
        folder = await DoCreateFolderAsync(from, toFolder, ct, null).ConfigureAwait(false);
        result = new GetOrCreateFolder(folder, true);
        progressPayload.Status = string.Format("Creating folder '{0}' under {1}...", from.Name, toFolder.Name);
      }

      progressPayload.Count++;
      progressPayload.Current = folder;
      progress.TryReport(() => progressPayload);
      return result;
    }

    private async Task<T> GetFileAsync<T>(string name, IGoogleDriveFolder toFolder, CancellationToken ct, IProgress<string> progress) where T : class, IGoogleDriveFile
    {
      var query = string.Format(CommonQueries.LiveFileFormat + " and name = '{1}'", toFolder.Id, name);
      var files = await DoGetGoogleDriveFilesAsync(toFolder, query, ct, progress).ConfigureAwait(false);
      if (files.Count == 1)
      {
        return files[0] as T;
      }

      if (files.Count > 1)
      {
        throw new ArgumentException(string.Format("More than one files found in folder '{0}', name: {1}", toFolder, name));
      }

      return null;
    }

    private async Task<IGoogleDriveFolder> DoCreateFolderAsync(IGoogleDriveFolder from, IGoogleDriveFolder toFolder, CancellationToken ct, IProgress<string> progress)
    {
      var fromCopy = new File {Parents = new List<string> {toFolder.Id}, Name = from.Name, MimeType = MimeType.Folder};
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);
      var createRequest = driveService.Files.Create(fromCopy);
      createRequest.SupportsTeamDrives = true;
      var resp = await createRequest.ExecuteAsync(ct).ConfigureAwait(false);
      var result = new GoogleDriveFolder(resp, toFolder, _ => DoGetChildFilesAsync(_, null, ct, progress));
      RaiseFileCreated(result);
      return result;
    }

    private async Task<IGoogleDriveFolder> DoCreateFolderAsync(string name, IGoogleDriveFolder parent, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      progress.TryReport("Creating folder '{0}' under folder '{1}'...", name, parent.Name);
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);
      try
      {
        var folder = new File { Name = name, MimeType = MimeType.Folder, Parents = new List<string> { parent.Id } };
        var createRequest = driveService.Files.Create(folder);
        createRequest.SupportsTeamDrives = true;
        var resp = await createRequest.ExecuteAsync(ct).ConfigureAwait(false);
        var result = (IGoogleDriveFolder)CreateDriveFile(resp, parent, ct, progress);
        RaiseFileCreated(result);
        return result;
      }
      catch (Exception ex)
      {
        Logger.Error("Error creating folder {0} - {1}", name, ex);
        return null;
      }
    }

    private async Task DoDeleteAsync(string id, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);
      try
      {
        var deleteRequest = driveService.Files.Delete(id);
        deleteRequest.SupportsTeamDrives = true;
        await deleteRequest.ExecuteAsync(ct).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        Logger.Error("Error deleting file {0} - {1}", id, ex);
      }
    }
    #endregion Private Methods

    /// <summary>
    /// GetOrCreateFolder
    /// </summary>
    private struct GetOrCreateFolder
    {
      public GetOrCreateFolder(IGoogleDriveFolder folder, bool isNew)
      {
        Folder = folder;
        IsNew = isNew;
      }

      public readonly IGoogleDriveFolder Folder;

      public readonly bool IsNew;
    }
  }
}
