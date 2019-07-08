// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-21 9:14 PM

namespace eagleboost.googledrive.Services
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Reactive.Subjects;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.core.Extensions;
  using eagleboost.core.Logging;
  using eagleboost.core.Threading;
  using eagleboost.core.Utils;
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Extensions;
  using eagleboost.googledrive.Models;
  using eagleboost.googledrive.Types;
  using Google.Apis.Download;
  using Google.Apis.Drive.v3;
  using Google.Apis.Drive.v3.Data;
  using Google.Apis.DriveActivity.v2;
  using Google.Apis.DriveActivity.v2.Data;
  using Google.Apis.Services;
  using File = Google.Apis.Drive.v3.Data.File;
  using User = Google.Apis.DriveActivity.v2.Data.User;

  /// <summary>
  /// GoogleDriveService
  /// </summary>
  public partial class GoogleDriveService : IGoogleDriveService
  {
    #region Declarations
    private readonly string _credentialsFile;
    private readonly string _credentialTokenFile;
    private readonly string _activityCredentialsFile;
    private readonly string _activityCredentialTokenFile;
    private readonly string _applicationName;
    private TaskCompletionSource<DriveService> _driveServiceTcs;
    private readonly object _driveServiceTcsLock = new object();
    private TaskCompletionSource<DriveActivityService> _driveActivityServiceTcs;
    private readonly object _driveActivityServiceTcsLock = new object();
    private readonly ILoggerFacade _logger;
    private Subject<IReadOnlyCollection<IGoogleDriveFile>> _changesSubject;
    private Subject<IReadOnlyCollection<GoogleDriveActivity>> _activitiesSubject;
    private readonly IGoogleDriveFolder _rootFolder;
    #endregion Declarations

    #region ctors
    public GoogleDriveService(string credentialsFile, string credentialTokenFile, string activityCredentialsFile, string activityCredentialTokenFile, string applicationName) 
      : this(null, credentialsFile, credentialTokenFile, activityCredentialsFile, activityCredentialTokenFile, applicationName)
    {
    }

    public GoogleDriveService(ILoggerFacade logger, string credentialsFile, string credentialTokenFile, string activityCredentialsFile, string activityCredentialTokenFile, string applicationName)
    {
      _logger = logger ?? LoggerManager.GetLogger<GoogleDriveService>();
      _credentialsFile = credentialsFile;
      _credentialTokenFile = credentialTokenFile;
      _activityCredentialsFile = activityCredentialsFile;
      _activityCredentialTokenFile = activityCredentialTokenFile;
      _applicationName = applicationName;
      _rootFolder = new GoogleDriveFolder(new GoogleMyDrive(), null, f => null);
    }
    #endregion ctors

    #region Private Properties
    private ILoggerFacade Logger
    {
      get { return _logger; }
    }
    #endregion Private Properties

    #region IGoogleDriveService
    public string StartPageToken { get; set; }

    public IObservable<IReadOnlyCollection<IGoogleDriveFile>> ObserveChanges(string startPageToken)
    {
      if (_changesSubject == null)
      {
        _changesSubject = new Subject<IReadOnlyCollection<IGoogleDriveFile>>();
        if (startPageToken != null)
        {
          StartPageToken = startPageToken;
          Task.Run(() => RetrieveChangesAsync(_changesSubject));
        }
        else
        {
          Task.Run(async () =>
          {
            var driveService = await GetDriveServiceAsync().ConfigureAwait(false);
            var req = driveService.Changes.GetStartPageToken();
            var token = await req.ExecuteAsync();
            StartPageToken = token.StartPageTokenValue;
            await RetrieveChangesAsync(_changesSubject);
          });
        }
      }

      return _changesSubject;
    }

    public IObservable<IReadOnlyCollection<GoogleDriveActivity>> ObserveActivities(CancellationToken ct = default(CancellationToken))
    {
      if (_activitiesSubject == null)
      {
        _activitiesSubject = new Subject<IReadOnlyCollection<GoogleDriveActivity>>();
        Task.Run(() => ObserveActivitiesAsync(_activitiesSubject, ct));
      }

      return _activitiesSubject;
    }

    private async Task ObserveActivitiesAsync(Subject<IReadOnlyCollection<GoogleDriveActivity>> subject, CancellationToken ct)
    {
      var activityService = await GetDriveActivityServiceAsync().ConfigureAwait(false);

      string pageToken = null;
      do
      {
        var requestData = new QueryDriveActivityRequest { PageSize = 10 };
        var query = activityService.Activity.Query(requestData);
        var response = await query.ExecuteAsync().ConfigureAwait(false);
        var activities = response.Activities;
        if (activities == null || activities.Count == 0)
        {
          Logger.Info("No activities");
          subject.OnCompleted();
          return;
        }

        var googleActivities = new List<GoogleDriveActivity>(activities.Count);
        foreach (var activity in activities)
        {
          string time = GetTimeInfo(activity);
          string action = GetActionInfo(activity.PrimaryActionDetail);
          List<string> actors = activity.Actors.Select(a => GetActorInfo(a)).ToList();
          List<string> targetNames = activity.Targets.Select(t => GetTargetName(t)).ToList();
          List<string> targetIds = activity.Targets.Select(t => GetTargetId(t)).ToList();
          var ga = new GoogleDriveActivity(Truncated(actors), action, Truncated(targetNames), Truncated(targetIds), time);
          Logger.Info("New activity: " + ga);
          googleActivities.Add(ga);
        }
        subject.OnNext(googleActivities);

        pageToken = response.NextPageToken;
      }
      while (pageToken != null && !ct.IsCancellationRequested);

      subject.OnCompleted();
    }

    // Returns a string representation of the first elements in a list.
    private static string Truncated<T>(List<T> list, int limit = 2)
    {
      string contents = string.Join(", ", list.Take(limit));
      string more = list.Count > limit ? ", ..." : "";
      return string.Format("[{0}{1}]", contents, more);
    }

    // Returns the name of a set property in an object, or else "unknown".
    private static string GetOneOf(object obj)
    {
      foreach (var p in obj.GetType().GetProperties())
      {
        if (!ReferenceEquals(p.GetValue(obj), null))
        {
          return p.Name;
        }
      }
      return "unknown";
    }

    // Returns a time associated with an activity.
    private static string GetTimeInfo(DriveActivity activity)
    {
      if (activity.Timestamp != null)
      {
        return activity.Timestamp.ToString();
      }
      if (activity.TimeRange != null)
      {
        return activity.TimeRange.EndTime.ToString();
      }
      return "unknown";
    }

    // Returns the type of action.
    private static string GetActionInfo(ActionDetail actionDetail)
    {
      return GetOneOf(actionDetail);
    }

    // Returns user information, or the type of user if not a known user.
    private static string GetUserInfo(User user)
    {
      if (user.KnownUser != null)
      {
        KnownUser knownUser = user.KnownUser;
        bool isMe = knownUser.IsCurrentUser ?? false;
        return isMe ? "people/me" : knownUser.PersonName;
      }
      return GetOneOf(user);
    }

    // Returns actor information, or the type of actor if not a user.
    private static string GetActorInfo(Actor actor)
    {
      if (actor.User != null)
      {
        return GetUserInfo(actor.User);
      }
      return GetOneOf(actor);
    }

    // Returns the type of a target and an associated title.
    private static string GetTargetName(Target target)
    {
      if (target.DriveItem != null)
      {
        return "driveItem:\"" + target.DriveItem.Title + "\"";
      }
      if (target.Drive != null)
      {
        return "drive:\"" + target.Drive.Title + "\"";
      }
      if (target.FileComment != null)
      {
        DriveItem parent = target.FileComment.Parent;
        if (parent != null)
        {
          return "fileComment:\"" + parent.Title + "\"";
        }
        return "fileComment:unknown";
      }
      return GetOneOf(target);
    }

    private static string GetTargetId(Target target)
    {
      if (target.DriveItem != null)
      {
        return "driveItem:\"" + target.DriveItem.Name + "\"";
      }
      if (target.Drive != null)
      {
        return "drive:\"" + target.Drive.Name + "\"";
      }
      if (target.FileComment != null)
      {
        DriveItem parent = target.FileComment.Parent;
        if (parent != null)
        {
          return "fileComment:\"" + parent.Name + "\"";
        }
        return "fileComment:unknown";
      }
      return GetOneOf(target);
    }

    private async Task RetrieveChangesAsync(Subject<IReadOnlyCollection<IGoogleDriveFile>> subject)
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);

      var pageToken = StartPageToken;
      while (pageToken != null)
      {
        var request = driveService.Changes.List(pageToken);
        request.Spaces = "drive";
        request.IncludeTeamDriveItems = true;
        request.IncludeCorpusRemovals = true;
        request.IncludeRemoved = true;
        request.SupportsTeamDrives = true;
        var changes = await request.ExecuteAsync();
        foreach (var change in changes.Changes)
        {
          // Process change
          var log = "Change found for file: " + change.FileId;
          if (change.File != null)
          {
            log += ", " + change.File.Name;
          }
          Logger.Info(log);
        }

        var changedFiles = changes.Changes.Select(CreateChange).ToArray();
        subject.OnNext(changedFiles);

        if (changes.NewStartPageToken != null)
        {
          // Last page, save this token for the next polling interval
          StartPageToken = changes.NewStartPageToken;
        }
        pageToken = changes.NextPageToken;
      }
    }

    private IGoogleDriveFile CreateChange(Change change)
    {
      var file = change.File;
      if (file != null)
      {
        return CreateDriveFile(file, null, default(CancellationToken), null);
      }

      return new GoogleDriveFileUnknownChange(change);
    }

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

    public Task<IGoogleDriveFile> GetFileAsync(string id, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(() => DoGetGoogleDriveFileAsync(id, ct, progress), ct);
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

    public Task DownloadAsync(string id, Stream stream, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(() => DoDownloadAsync(id, stream, ct, progress), ct);
    }

    public Task AddToSharedAsync(string fileId, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(() => DoAddToSharedAsync(fileId, ct, progress), ct);
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
      lock (_driveServiceTcsLock)
      {
        if (_driveServiceTcs == null)
        {
          _driveServiceTcs = new TaskCompletionSource<DriveService>();
          Task.Run(async () =>
          {
            var provider = new UserCredentialProvider();
            var credential = await provider.GetUserCredentialAsync(_credentialsFile, _credentialTokenFile).ConfigureAwait(true);
            var driveService = new DriveService(new BaseClientService.Initializer
            {
              HttpClientInitializer = credential,
              ApplicationName = _applicationName,
            });

            _driveServiceTcs.TrySetResult(driveService);
          });
        }

        return _driveServiceTcs.Task;
      }
    }

    private Task<DriveActivityService> GetDriveActivityServiceAsync()
    {
      lock (_driveActivityServiceTcsLock)
      {
        if (_driveActivityServiceTcs == null)
        {
          _driveActivityServiceTcs = new TaskCompletionSource<DriveActivityService>();
          Task.Run(async () =>
          {
            var provider = new UserCredentialProvider();
            var credential = await provider.GetUserCredentialAsync(_activityCredentialsFile, _activityCredentialTokenFile).ConfigureAwait(true);
            var driveService = new DriveActivityService(new BaseClientService.Initializer
            {
              HttpClientInitializer = credential,
              ApplicationName = _applicationName,
            });

            _driveActivityServiceTcs.TrySetResult(driveService);
          });
        }

        return _driveActivityServiceTcs.Task;
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

    private async Task<IGoogleDriveFile> DoGetGoogleDriveFileAsync(string id, CancellationToken ct, IProgress<string> progress)
    {
      var f = await DoGetFileAsync(id, ct, progress).ConfigureAwait(false);
      var result = CreateDriveFile(f, null, ct, progress);
      return result;
    }

    private async Task<File> DoGetFileAsync(string id, CancellationToken ct, IProgress<string> progress)
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);
      var request = driveService.GetFileRequest(id);
      var file = await request.ExecuteAsync(ct);
      progress.TryReport("Loaded file ", file.Name + "[" + id + "]");
      return file;
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
      var getOrCreateFolder = await GetOrCreateFolderAsync(from.Name, toFolder, ct, progress, progressPayload).ConfigureAwait(false);
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

    private async Task<GetOrCreateFolder> GetOrCreateFolderAsync(string name, IGoogleDriveFolder parent,
      CancellationToken ct, IProgress<GoogleDriveProgress> progress = null, GoogleDriveProgress progressPayload = null)
    {
      GetOrCreateFolder result;
      var folder = await GetFileAsync<IGoogleDriveFolder>(name, parent, ct, null).ConfigureAwait(false);
      if (folder != null)
      {
        result = new GetOrCreateFolder(folder, false);
        if (progressPayload != null)
        {
          progressPayload.Status = string.Format("Found folder '{0}' under {1}...", name, parent.Name);
        }
      }
      else
      {
        folder = await DoCreateFolderAsync(name, parent, ct, null).ConfigureAwait(false);
        result = new GetOrCreateFolder(folder, true);
        if (progressPayload != null)
        {
          progressPayload.Status = string.Format("Creating folder '{0}' under {1}...", name, parent.Name);
        }
      }

      if (progressPayload != null)
      {
        progressPayload.Count++;
        progressPayload.Current = folder;
        progress.TryReport(() => progressPayload);
      }

      return result;
    }

    private async Task<T> GetFileAsync<T>(string name, IGoogleDriveFolder toFolder, CancellationToken ct, IProgress<string> progress) where T : class, IGoogleDriveFile
    {
      var query = string.Format(CommonQueries.LiveFileFormat + " and name = '{1}'", toFolder.Id, NormalizeName(name));
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

    private string NormalizeName(string name)
    {
      return name.Replace("'", "\\'");
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
        var result = new GoogleDriveFolder(resp, parent, _ => DoGetChildFilesAsync(_, null, ct, progress));
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

    private async Task DoDownloadAsync(string id, Stream stream, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      ArgValidation.ThrowIfNull(stream, "stream");

      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);
      try
      {
        var request = driveService.Files.Get(id);
        request.SupportsTeamDrives = true;
        if (progress != null)
        {
          request.MediaDownloader.ProgressChanged += p =>
          {
            switch (p.Status)
            {
              case DownloadStatus.Downloading:
              {
                progress.Report("Bytes downloaded: "+p.BytesDownloaded);
                break;
              }
              case DownloadStatus.Completed:
              {
                progress.Report("Download completed");
                break;
              }
              case DownloadStatus.Failed:
              {
                progress.Report("Download failed");
                break;
              }
            }
          };
        }

        await request.DownloadAsync(stream, ct).ConfigureAwait(false);
        stream.Seek(0, SeekOrigin.Begin);
      }
      catch (Exception ex)
      {
        Logger.Error("Error deleting file {0} - {1}", id, ex);
      }
    }

    private async Task DoAddToSharedAsync(string fileId, CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      var driveService = await GetDriveServiceAsync().ConfigureAwait(false);
      var sharedFolder = await GetOrCreateFolderAsync("_Shared", _rootFolder, ct);
      var sharedFolderId = sharedFolder.Folder.Id;
      var file = await DoGetGoogleDriveFileAsync(fileId, ct, progress);
      if (file.OwnedByMe)
      {
        Logger.Info(file.Name + "[" + fileId + "] is not shared by others");
        return;
      }

      if (file.Parents.Contains(sharedFolderId))
      {
        Logger.Info(file.Name + "[" + fileId + "] already exists in " + sharedFolder.Folder);
        return;
      }

      var updateRequest = driveService.Files.Update(new File(), fileId);
      updateRequest.Fields = "id, parents";
      updateRequest.AddParents = sharedFolder.Folder.Id;
      await updateRequest.ExecuteAsync(ct);
      Logger.Info("Added " + file.Name + "[" + fileId + "]  to " + sharedFolder.Folder);
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
