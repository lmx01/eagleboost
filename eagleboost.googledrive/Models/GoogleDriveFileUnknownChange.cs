// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 06 8:40 PM

namespace eagleboost.googledrive.Models
{
  using System;
  using System.Collections.Generic;
  using eagleboost.googledrive.Contracts;
  using eagleboost.shell.FileSystems.Models;
  using Google.Apis.Drive.v3.Data;

  /// <summary>
  /// GoogleDriveFileUnknownChange
  /// </summary>
  public class GoogleDriveFileUnknownChange : FileBase<GoogleDriveFileUnknownChange, IGoogleDriveFolder>, IGoogleDriveFile
  {
    #region Declarations
    private readonly Change _change;
    #endregion Declarations

    #region ctors
    public GoogleDriveFileUnknownChange(Change change) : base(null)
    {
      _change = change;
    }
    #endregion ctors

    #region Overrides
    public override string Id
    {
      get { return _change.FileId; }
    }

    public override string Name
    {
      get { return null; }
    }

    public override string Type
    {
      get { return _change.Type; }
    }

    public override long? Size
    {
      get { return null; }
    }

    public override DateTime? CreatedTime
    {
      get { return null; }
    }

    public override DateTime? ModifiedTime
    {
      get { return null; }
    }
    #endregion Overrides

    #region IGoogleDriveFile
    public bool OwnedByMe
    {
      get { return false; }
    }

    public string Owners
    {
      get { return null; }
    }

    public string Parents
    {
      get { return null; }
    }

    public bool? IsChildrenCopied
    {
      get { return null; }
    }

    public string WebContentLink
    {
      get { return null; }
    }

    public string WebViewLink
    {
      get { return null; }
    }

    public string IconLink
    {
      get { return null; }
    }

    public bool? HasThumbnail
    {
      get { return null; }
    }

    public string ThumbnailLink
    {
      get { return null; }
    }

    public IDictionary<string, string> AppProperties
    {
      get { return null; }
    }
    #endregion IGoogleDriveFile
  }
}