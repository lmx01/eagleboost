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
  using eagleboost.core.Logging;
  using eagleboost.googledrive.Models;
  using Google.Apis.Drive.v3.Data;

  /// <summary>
  /// GoogleDriveService
  /// </summary>
  public partial class GoogleDriveService
  {
    public IObservable<IReadOnlyCollection<GoogleDriveActivity>> ObserveChanges(string startPageToken)
    {
      if (_changesSubject == null)
      {
        _changesSubject = new Subject<IReadOnlyCollection<GoogleDriveActivity>>();
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

    private async Task RetrieveChangesAsync(Subject<IReadOnlyCollection<GoogleDriveActivity>> subject)
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
        var changedFiles = new List<GoogleDriveActivity>();
        foreach (var change in changes.Changes)
        {
          // Process change
          var log = "Change found for file: " + change.FileId;
          if (change.File != null)
          {
            var gFile = await GetFileAsync(change.FileId, null).ConfigureAwait(false);
            log += ", " + gFile.Name + "[" + gFile.LastModifyingUser + "]";
          }
          Logger.Info(log);
          var file = CreateChange(change);
          changedFiles.Add(file);
        }

        subject.OnNext(changedFiles);

        if (changes.NewStartPageToken != null)
        {
          // Last page, save this token for the next polling interval
          StartPageToken = changes.NewStartPageToken;
        }
        pageToken = changes.NextPageToken;
      }
    }

    private GoogleDriveActivity CreateChange(Change change)
    {
      var file = change.File;
      if (file != null)
      {
        var gFile = CreateDriveFile(file, null, default(CancellationToken), null);
        return new GoogleDriveActivity(gFile.LastModifyingUser, "", file.Name, file.Id, gFile);
      }

      return new GoogleDriveActivity("Unknown", "", "N/A", change.FileId, new GoogleDriveFileUnknownChange(change));
    }
  }
}