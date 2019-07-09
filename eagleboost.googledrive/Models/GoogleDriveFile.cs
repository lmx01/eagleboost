// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-21 9:15 PM

namespace eagleboost.googledrive.Models
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using eagleboost.googledrive.Contracts;
  using eagleboost.shell.FileSystems.Models;
  using Google.Apis.Drive.v3.Data;

  /// <summary>
  /// GoogleDriveFile
  /// </summary>
  public class GoogleDriveFile : FileBase<GoogleDriveFile, IGoogleDriveFolder>, IGoogleDriveFile
  {
    #region Declarations
    private readonly File _file;
    #endregion Declarations

    #region ctors
    public GoogleDriveFile(File file, IGoogleDriveFolder parent) : base(parent)
    {
      _file = file;
    }
    #endregion ctors

    #region Public Properties
    public File File
    {
      get { return _file; }
    }
    #endregion Public Properties

    #region Overrides
    public override string Id
    {
      get { return _file.Id; }
    }

    public override string Name
    {
      get { return _file.Name; }
    }

    public override string Type
    {
      get { return _file.MimeType; }
    }

    public override long? Size
    {
      get { return _file.Size; }
    }

    public override DateTime? CreatedTime
    {
      get { return _file.CreatedTime; }
    }

    public override DateTime? ModifiedTime
    {
      get { return _file.ModifiedTime; }
    }

    public override string LastModifyingUser
    {
      get
      {
        var user = _file.LastModifyingUser;
        return user != null
          ? user.Me.GetValueOrDefault(false) ? "me" : user.DisplayName
          : null;
      }
    }
    #endregion Overrides

    #region IGoogleDriveFile
    public bool OwnedByMe
    {
      get { return _file.OwnedByMe.GetValueOrDefault(false); }
    }

    public string Owners
    {
      get { return _file.Owners != null ? string.Join(",", _file.Owners.Select(i => i.DisplayName)) : null; }
    }

    public string Parents
    {
      get { return _file.Parents != null ? string.Join(",", _file.Parents) : null; }
    }

    public bool? IsChildrenCopied
    {
      get { return null; }
    }

    public string WebContentLink
    {
      get { return _file.WebContentLink; }
    }

    public string WebViewLink
    {
      get { return _file.WebViewLink; }
    }

    public string IconLink
    {
      get { return _file.IconLink; }
    }

    public bool? HasThumbnail
    {
      get { return _file.HasThumbnail; }
    }

    public string ThumbnailLink
    {
      get { return _file.ThumbnailLink; }
    }

    public IDictionary<string, string> AppProperties
    {
      get { return _file.AppProperties; }
    }
    #endregion IGoogleDriveFile
  }
}