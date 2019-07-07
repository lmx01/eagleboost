// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-21 9:48 PM

namespace eagleboost.googledrive.Extensions
{
  using System.Linq;
  using Google.Apis.Drive.v3;

  public static class DriveServiceExt
  {
    public static string[] DefaultFileFields = new[]
    {
      "id",
      "name",
      "mimeType",
      "ownedByMe",
      "owners",
      "parents",
      "size",
      "appProperties",
      "createdTime",
      "modifiedTime",
      "webContentLink",
      "webViewLink",
      "iconLink",
      "hasThumbnail",
      "thumbnailLink",
    };

    public static FilesResource.ListRequest GetListRequest(this DriveService driveService, string query, string pageToken, params string[] fileFields)
    {
      var request = driveService.Files.List();
      request.Q = query;
      request.Spaces = "drive";
      request.Corpora = "user";
      request.IncludeTeamDriveItems = true;
      request.SupportsTeamDrives = true;
      var fields = fileFields.Any() ? fileFields : DefaultFileFields;
      request.Fields = "nextPageToken, files(" + string.Join(", ", fields) + ")";
      request.OrderBy = "name";
      request.PageToken = pageToken;

      return request;
    }

    public static FilesResource.GetRequest GetFileRequest(this DriveService driveService, string id, params string[] fileFields)
    {
      var request = driveService.Files.Get(id);
      var fields = fileFields.Any() ? fileFields : DefaultFileFields;
      request.Fields = string.Join(", ", fields);
      return request;
    }
  }
}