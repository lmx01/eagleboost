// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-21 9:48 PM

namespace eagleboost.googledrive.Extensions
{
  using Google.Apis.Drive.v3;

  public static class DriveServiceExt
  {
    public static FilesResource.ListRequest GetListRequest(this DriveService driveService, string query, string pageToken)
    {
      var request = driveService.Files.List();
      request.Q = query;
      request.Spaces = "drive";
      request.Corpora = "user";
      request.Fields = "nextPageToken, files(id, name, mimeType, ownedByMe, owners, size, appProperties)";
      request.OrderBy = "name";
      request.PageToken = pageToken;

      return request;
    }
  }
}