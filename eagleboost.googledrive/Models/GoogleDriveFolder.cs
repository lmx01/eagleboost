// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-21 9:15 PM

namespace eagleboost.googledrive.Models
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.googledrive.Contracts;
  using eagleboost.shell.FileSystems.Contracts;
  using eagleboost.shell.FileSystems.Models;
  using Google.Apis.Drive.v3.Data;

  /// <summary>
  /// GoogleDriveFolder
  /// </summary>
  public class GoogleDriveFolder : FileBase<GoogleDriveFolder, IGoogleDriveFolder>, IGoogleDriveFolder
  {
    #region Declarations
    private readonly Func<GoogleDriveFolder, Task<IReadOnlyList<IGoogleDriveFile>>> _filesFunc;
    private IReadOnlyList<IGoogleDriveFile> _filesCache;
    private readonly File _file;
    #endregion Declarations

    #region ctors
    public GoogleDriveFolder(File file, IGoogleDriveFolder parent, Func<GoogleDriveFolder, Task<IReadOnlyList<IGoogleDriveFile>>> filesFunc) : base(parent)
    {
      _file = file;
      _filesFunc = filesFunc;
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
    #endregion Overrides

    #region IGoogleDriveFile
    public bool OwnedByMe
    {
      get { return _file.OwnedByMe.GetValueOrDefault(false); }
    }

    public string Owners
    {
      get { return string.Join(",", _file.Owners.Select(i => i.DisplayName)); }
    }

    public IDictionary<string, string> AppProperties
    {
      get { return _file.AppProperties; }
    }
    #endregion IGoogleDriveFile

    #region IGoogleDriveFolder
    public async Task<IReadOnlyList<IFile>> GetFilesAsync(bool refresh = false, CancellationToken ct = new CancellationToken())
    {
      if (!refresh && _filesCache != null)
      {
        return _filesCache;
      }

      return _filesCache = await _filesFunc(this).ConfigureAwait(false);
    }
    #endregion IGoogleDriveFolder
  }
}