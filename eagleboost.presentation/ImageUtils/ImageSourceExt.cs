namespace eagleboost.presentation.ImageUtils
{
  using System;
  using System.IO;
  using System.Windows;
  using System.Windows.Media;
  using System.Windows.Media.Imaging;

  public static class ImageSourceExt
  {
    public static void SaveToPng(this ImageSource img, string fileName)
    {
      SaveToFile(BitmapFrame.Create((BitmapSource) img), new PngBitmapEncoder(), fileName);
    }

    public static void SaveToJpg(this BitmapFrame img, string fileName)
    {
      SaveToFile(img, new JpegBitmapEncoder(), fileName);
    }

    private static void SaveToFile(this BitmapFrame img, BitmapEncoder encoder, string fileName)
    {
      encoder.Frames.Add(img);

      using (var filestream = new FileStream(fileName, FileMode.Create))
      {
        encoder.Save(filestream);
      }
    }

    public static ImageSource GetThumbnail(string uri, int size, bool square = false)
    {
      var result = (BitmapImage)ResizeImage(uri, size);
      if (!square)
      {
        return result;
      }

      var centerCropRect = GetCenterCropRect(result);
      return new CroppedBitmap(result, centerCropRect);
    }

    public static ImageSource ResizeImage(string uri, int size)
    {
      var thumbnailImage = new BitmapImage();

      thumbnailImage.BeginInit();
      thumbnailImage.UriSource = new Uri(uri);
      thumbnailImage.DecodePixelWidth = size;
      thumbnailImage.EndInit();

      return thumbnailImage;
    }

    public static BitmapFrame CreateResizeImage(string uri, double size)
    {
      var photoDecoder = BitmapDecoder.Create(new Uri(uri, UriKind.Absolute),
        BitmapCreateOptions.PreservePixelFormat,
        BitmapCacheOption.None);
      var photo = photoDecoder.Frames[0];

      var width = photo.PixelWidth;
      var height = photo.PixelHeight;

      var ratio = height > width ? size / width : size / height;
      var target = new TransformedBitmap(photo, new ScaleTransform(ratio, ratio, 0, 0));

      return BitmapFrame.Create(target);
      //var image = new BitmapImage();

      //image.BeginInit();
      //image.UriSource = new Uri(uri);
      //image.EndInit();

      //var width = image.Width;
      //var height = image.Height;

      //if (size > width || size > height)
      //{
      //  size = Math.Min(width, height);
      //}

      //var newWidth = (int)size;
      //var newHeight = (int)size;
      //if (IsVertical(image))
      //{
      //  newHeight = (int)(newWidth * height / width);
      //}
      //else
      //{
      //  newWidth = (int)(newHeight * width / height);
      //}

      //var rect = new Rect(0, 0, width, height);

      //var group = new DrawingGroup();
      //RenderOptions.SetBitmapScalingMode(group, BitmapScalingMode.HighQuality);
      //group.Children.Add(new ImageDrawing(image, rect));

      //var drawingVisual = new DrawingVisual();
      //using (var drawingContext = drawingVisual.RenderOpen())
      //  drawingContext.DrawDrawing(group);

      //var resizedImage = new RenderTargetBitmap(
      //  newWidth, newHeight,         // Resized dimensions
      //  image.DpiX, image.DpiY,                // Default DPI values
      //  PixelFormats.Default); // Default pixel format
      //resizedImage.Render(drawingVisual);

      //return BitmapFrame.Create(resizedImage);
    }

    private static bool IsVertical(BitmapImage image)
    {
      return image.PixelHeight > image.PixelWidth;
    }

    private static Int32Rect GetCenterCropRect(BitmapImage image)
    {
      if (IsVertical(image))
      {
        var size = image.PixelWidth - 1;
        var top = (image.PixelHeight - image.PixelWidth) / 2;
        return new Int32Rect(0, top, size, size);
      }
      else
      {
        var size = image.PixelHeight - 1;
        var left = (image.PixelWidth - image.PixelHeight) / 2;
        return new Int32Rect(left, 0, size, size);
      }
    }
  }
}