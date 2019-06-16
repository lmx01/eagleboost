// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 15 5:32 PM

namespace eagleboost.googledrive.Models
{
  using eagleboost.googledrive.Contracts;

  public class GoogleDriveFileInfoModel
  {
    #region Declarations
    private readonly IGoogleDriveFile _file;
    #endregion Declarations

    #region ctors
    public GoogleDriveFileInfoModel(IGoogleDriveFile file)
    {
      _file = file;
    }
    #endregion ctors

    #region Public Properties
    public IGoogleDriveFile File
    {
      get { return _file; }
    }

    public string FileName
    {
      get { return _file.Name; }
    }

    #endregion Public Properties
  }
}