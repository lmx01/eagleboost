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
  using eagleboost.core.Extensions;
  using eagleboost.core.Logging;
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Models;
  using eagleboost.googledrive.Types;
  using eagleboost.presentation.Tools;
  using eagleboost.shell.FileSystems.Contracts;

  /// <summary>
  /// GoogleDriveFileInfoViewModel
  /// </summary>
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

    #region Overrides
    protected override void OnPropertyChanging(string propertyName)
    {
      base.OnPropertyChanging(propertyName);

      if (propertyName == this.Property(o=>o.FileInfo))
      {
        DisposeImageStream(FileInfo as GoogleDriveImageFileInfoModel);
      }
    }

    #endregion Overrides

    #region Private Methods
    private Task<GoogleDriveFileInfoModel> DoLoadFileInfoAsync(IGoogleDriveFile file, CancellationToken ct,IProgress<string> process)
    {
      if (file.Type == MimeType.Jpeg || file.Type == MimeType.Png)
      {
        return DoLoadImageInfoAsync(file, ct, process);
      }

      if (file.Type == MimeType.Gif)
      {
        return DoLoadStreamInfoAsync(file, ct, process);
      }

      if (file.HasThumbnail.GetValueOrDefault() && file.ThumbnailLink.HasValue())
      {
        return DoLoadThumbnailInfoAsync(file);
      }

      return Task.FromResult(new GoogleDriveFileInfoModel(file));
    }

    private GoogleDriveImageFileInfoModel CreateFileInfo(IGoogleDriveFile file, BitmapSource imageSource)
    {
      var fileInfo = new GoogleDriveImageFileInfoModel(file)
      {
        ImageSource = imageSource,
        Width = imageSource.PixelWidth,
        Height = imageSource.PixelHeight,
      };
      return fileInfo;
    }

    private async Task<GoogleDriveFileInfoModel> DoLoadStreamInfoAsync(IGoogleDriveFile file, CancellationToken ct, IProgress<string> process)
    {
      var stream = new MemoryStream();
      await _gService.DownloadAsync(file.Id, stream, ct, process);
      var imageSource = await LoadThumbnailAsync(file, stream);
      var fileInfo = CreateFileInfo(file, imageSource);
      fileInfo.ImageStream = stream;
      return fileInfo;
    }

    private async Task<BitmapSource> LoadThumbnailAsync(IGoogleDriveFile file, Stream stream)
    {
      if (file.ThumbnailLink.HasValue())
      {
        return await WebImageDownloader.DownloadAsync(file.ThumbnailLink);
      }

      return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
    }

    private async Task<GoogleDriveFileInfoModel> DoLoadImageInfoAsync(IGoogleDriveFile file, CancellationToken ct, IProgress<string> process)
    {
      using (var stream = new MemoryStream())
      {
        await _gService.DownloadAsync(file.Id, stream, ct, process);
        var imageSource = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
        return CreateFileInfo(file, imageSource);
      }
    }

    private async Task<GoogleDriveFileInfoModel> DoLoadThumbnailInfoAsync(IGoogleDriveFile file)
    {
      var imageSource = await WebImageDownloader.DownloadAsync(file.ThumbnailLink);
      return CreateFileInfo(file, imageSource);
    }

    private void DisposeImageStream(GoogleDriveImageFileInfoModel fileInfo)
    {
      if (fileInfo != null && fileInfo.ImageStream != null)
      {
        fileInfo.ImageStream.Dispose();
        fileInfo.ImageStream = null;
      }
    }
    #endregion Private Methods
  }
}