// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-21 3:55 PM

namespace eagleboost.googledrive.Contracts
{
  using System.Collections.Generic;
  using eagleboost.shell.FileSystems.Contracts;
  using Google.Apis.Drive.v3.Data;

  /// <summary>
  /// IGoogleDriveFile
  /// https://developers.google.com/drive/api/v3/reference/files
  /// </summary>
  public interface IGoogleDriveFile : IFile
  {
    #region Properties
    File File { get; }

    bool OwnedByMe { get; }

    string Owners { get; }

    string Parents { get; }

    bool? IsChildrenCopied { get; }

    string WebContentLink { get; }

    string WebViewLink { get; }

    string IconLink { get; }

    bool? HasThumbnail { get; }

    string ThumbnailLink { get; }

    IDictionary<string, string> AppProperties { get; }
    #endregion Properties
  }
}