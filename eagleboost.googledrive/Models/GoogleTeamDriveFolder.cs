// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 04 10:56 PM

namespace eagleboost.googledrive.Models
{
  using System;
  using System.Collections.Generic;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.googledrive.Contracts;
  using eagleboost.shell.FileSystems.Contracts;
  using eagleboost.shell.FileSystems.Models;
  using Google.Apis.Drive.v3.Data;

  public class GoogleTeamDriveFolder : FileBase<GoogleTeamDriveFolder, IGoogleDriveFolder>, IGoogleDriveFolder
  {
    #region Declarations
    private readonly Func<GoogleTeamDriveFolder, Task<IReadOnlyCollection<IGoogleDriveFile>>> _filesFunc;
    private readonly TeamDrive _teamDrive;
    private readonly IDictionary<string, string> _appProperties;
    #endregion Declarations

    #region ctors
    public GoogleTeamDriveFolder(TeamDrive teamDrive, IGoogleDriveFolder parent, Func<GoogleTeamDriveFolder, Task<IReadOnlyCollection<IGoogleDriveFile>>> filesFunc) : base(parent)
    {
      _appProperties = new Dictionary<string, string>();
      _teamDrive = teamDrive;
      _filesFunc = filesFunc;
    }
    #endregion ctors

    #region IGoogleDriveFolder
    public File File
    {
      get { return null; }
    }
    #endregion IGoogleDriveFolder

    #region Public Properties
    public TeamDrive TeamDrive
    {
      get { return _teamDrive; }
    }
    #endregion Public Properties

    #region Overrides
    public override string Id
    {
      get { return _teamDrive.Id; }
    }

    public override string Name
    {
      get { return _teamDrive.Name; }
    }

    public override string Type
    {
      get { return "Team Drive"; }
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

    public override string LastModifyingUser
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
      get { return _appProperties; }
    }
    #endregion IGoogleDriveFile

    #region IGoogleDriveFolder
    public async Task<IReadOnlyCollection<IFile>> GetFilesAsync(CancellationToken ct = new CancellationToken())
    {
      return await _filesFunc(this).ConfigureAwait(false);
    }
    #endregion IGoogleDriveFolder
  }
}