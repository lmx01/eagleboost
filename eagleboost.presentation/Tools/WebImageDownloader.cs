// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 03 8:45 PM

namespace eagleboost.presentation.Tools
{
  using System;
  using System.IO;
  using System.Net;
  using System.Threading.Tasks;
  using System.Windows.Media.Imaging;

  public class WebImageDownloader
  {
    public static Task<BitmapImage> DownloadAsync(string imageUri)
    {
      return Task.Run(() => DownloadImage(imageUri));
    }

    private static BitmapImage DownloadImage(string imageUri)
    {
      var image = new BitmapImage();
      var BytesToRead = 100;

      var request = WebRequest.Create(new Uri(imageUri, UriKind.Absolute));
      request.Timeout = -1;
      var response = request.GetResponse();
      var responseStream = response.GetResponseStream();
      var reader = new BinaryReader(responseStream);
      var memoryStream = new MemoryStream();

      var byteBuffer = new byte[BytesToRead];
      var bytesRead = reader.Read(byteBuffer, 0, BytesToRead);

      while (bytesRead > 0)
      {
        memoryStream.Write(byteBuffer, 0, bytesRead);
        bytesRead = reader.Read(byteBuffer, 0, BytesToRead);
      }

      image.BeginInit();
      memoryStream.Seek(0, SeekOrigin.Begin);

      image.StreamSource = memoryStream;
      image.EndInit();
      image.Freeze();

      return image;
    }
  }
}