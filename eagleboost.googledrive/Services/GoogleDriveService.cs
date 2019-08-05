// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-21 9:14 PM

namespace eagleboost.googledrive.Services
{
  using System;
  using System.Collections.Generic;
  using System.Reactive.Subjects;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.core.Logging;
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Models;
  using Google.Apis.Drive.v3;
  using Google.Apis.DriveActivity.v2;
  using Google.Apis.Services;

  /// <summary>
  /// GoogleDriveService
  /// </summary>
  public partial class GoogleDriveService : IGoogleDriveService
  {
    #region Statics
    private static string[] DriveScope = { DriveService.Scope.Drive, DriveService.Scope.DriveReadonly };
    private static string[] ActivityScope = { DriveActivityService.Scope.DriveActivity };
    #endregion Statics

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
    private Subject<IReadOnlyCollection<GoogleDriveActivity>> _changesSubject;
    private Subject<IReadOnlyCollection<GoogleDriveActivity>> _activitiesSubject;
    #endregion Declarations

    #region ctors
    public GoogleDriveService(string credentialsFile, string credentialTokenFile, string activityCredentialsFile, string activityCredentialTokenFile, string applicationName)
      : this(null, credentialsFile, credentialTokenFile, activityCredentialsFile, activityCredentialTokenFile, applicationName)
    {
    }

    public GoogleDriveService(ILoggerFacade logger, string credentialsFile, string credentialTokenFile, string activityCredentialsFile, string activityCredentialTokenFile, string applicationName)
    {
      Logger = logger ?? LoggerManager.GetLogger<GoogleDriveService>();
      _credentialsFile = credentialsFile;
      _credentialTokenFile = credentialTokenFile;
      _activityCredentialsFile = activityCredentialsFile;
      _activityCredentialTokenFile = activityCredentialTokenFile;
      _applicationName = applicationName;
    }
    #endregion ctors

    #region Private Properties
    private ILoggerFacade Logger { get; set; }
    #endregion Private Properties

    #region IGoogleDriveService
    public Task<IGoogleDriveFolder> GetMyDriveAsync(CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      return Task.Run(() => DoGetMyDriveAsync(ct, progress));
    }

    private async Task<IGoogleDriveFolder> DoGetMyDriveAsync(CancellationToken ct = default(CancellationToken), IProgress<string> progress = null)
    {
      var file = await DoGetFileAsync("root", ct, progress);
      return (IGoogleDriveFolder)CreateDriveFile(file, null, ct, progress);
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
            var credential = await provider.GetUserCredentialAsync(_credentialsFile, _credentialTokenFile, DriveScope).ConfigureAwait(true);
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
            var credential = await provider.GetUserCredentialAsync(_activityCredentialsFile, _activityCredentialTokenFile, ActivityScope).ConfigureAwait(true);
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
    #endregion IGoogleDriveService
  }
}
