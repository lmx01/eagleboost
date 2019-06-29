// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 15 5:29 PM

namespace eagleboost.googledrive.ViewModels
{
  using System;
  using System.IO;
  using System.Threading;
  using System.Threading.Tasks;
  using System.Windows.Media.Imaging;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.Logging;
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Models;
  using eagleboost.googledrive.Types;
  using eagleboost.shell.FileSystems.Contracts;

  public class GoogleDriveFileInfoViewModel : NotifyPropertyChangedBase, IFileSystemFileInfoViewModel<IGoogleDriveFile, IGoogleDriveFolder>
  {
    #region Statics
    private static readonly ILoggerFacade Log = LoggerManager.GetLogger<GoogleDriveFileInfoViewModel>();
    #endregion Statics

    #region Declarations
    private IGoogleDriveService _gService;
    private GoogleDriveFileInfoModel _fileInfo;
    #endregion Declarations

    #region Init
    public void Initialize(IGoogleDriveService gService)
    {
      _gService = gService;
    }
    #endregion Init

    #region Public Properties
    public GoogleDriveFileInfoModel FileInfo
    {
      get { return _fileInfo; }
      set { SetValue(ref _fileInfo, value); }
    }
    #endregion Public Properties

    #region IFileSystemFileInfoViewModel
    public async Task LoadFileInfoAsync(IGoogleDriveFile file, CancellationToken ct, IProgress<string> process)
    {
      try
      {
        var fileInfo = await DoLoadFileInfoAsync(file, ct, process);
        FileInfo = fileInfo;
      }
      catch (Exception ex)
      {
        Log.Warn("Failed to load file info: " + file + "\n" + ex);
        FileInfo = new GoogleDriveFileInfoInvalid(file);
      }
    }
    #endregion IFileSystemFileInfoViewModel

    #region Private Methods
    private async Task<GoogleDriveFileInfoModel> DoLoadFileInfoAsync(IGoogleDriveFile file, CancellationToken ct, IProgress<string> process)
    {
      if (file.Type == MimeType.Jpeg || file.Type == MimeType.Png)
      {
        using (var stream = new MemoryStream())
        {
          await _gService.DownloadAsync(file.Id, stream, ct, process);
          var imageSource =BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
          var fileInfo = new GoogleDriveImageFileInfoModel(file)
          {
            ImageSource = imageSource,
            Width = imageSource.PixelWidth,
            Height = imageSource.PixelHeight,
          };
          return fileInfo;
        }
      }

      return new GoogleDriveFileInfoModel(file);
    }
    #endregion Private Methods
  }
}