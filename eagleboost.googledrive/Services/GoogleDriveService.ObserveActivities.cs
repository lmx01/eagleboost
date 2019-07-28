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
  using eagleboost.core.Logging;
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Models;
  using Google.Apis.Drive.v3.Data;
  using Google.Apis.DriveActivity.v2.Data;
  using User = Google.Apis.DriveActivity.v2.Data.User;

  /// <summary>
  /// GoogleDriveService
  /// </summary>
  public partial class GoogleDriveService
  {
    public IObservable<IReadOnlyCollection<GoogleDriveActivity>> ObserveActivities(CancellationToken ct = default(CancellationToken))
    {
      if (_activitiesSubject == null)
      {
        _activitiesSubject = new Subject<IReadOnlyCollection<GoogleDriveActivity>>();
        Task.Run(() => ObserveActivitiesAsync(_activitiesSubject, ct), ct);
      }

      return _activitiesSubject;
    }

    /// <summary>
    /// https://dzone.com/articles/working-with-the-google-drive-api-track-changes-in
    /// https://developers.google.com/drive/activity/v2/quickstart/dotnet
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async Task ObserveActivitiesAsync(Subject<IReadOnlyCollection<GoogleDriveActivity>> subject, CancellationToken ct)
    {
      var activityService = await GetDriveActivityServiceAsync().ConfigureAwait(false);

      string pageToken;
      do
      {
        var requestData = new QueryDriveActivityRequest { PageSize = 10 };
        var query = activityService.Activity.Query(requestData);
        var response = await query.ExecuteAsync(ct).ConfigureAwait(false);
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
          var ga = new GoogleDriveActivity(Truncated(actors), action, Truncated(targetNames), Truncated(targetIds), null);
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
  }
}