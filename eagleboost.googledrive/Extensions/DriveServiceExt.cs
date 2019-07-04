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
      request.SupportsTeamDrives = true;
      request.IncludeTeamDriveItems = true;
      request.Spaces = "drive";
      request.Corpora = "user";
      var fields = fileFields.Any() ? fileFields : DefaultFileFields;
      request.Fields = "nextPageToken, files(" + string.Join(", ", fields) + ")";
      request.OrderBy = "name";
      request.PageToken = pageToken;

      return request;
    }
  }
}