// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 03 8:08 AM

namespace eagleboost.presentation.Tools
{
  using System.Collections.Concurrent;
  using System.Threading.Tasks;
  using System.Windows.Media;

  public class RemoteIconManager
  {
    private static readonly ConcurrentDictionary<string, Task<ImageSource>> TaskCache = new ConcurrentDictionary<string, Task<ImageSource>>();

    public static Task<ImageSource> GetIconAsync(string iconLink)
    {
      return TaskCache.GetOrAdd(iconLink, DoGetIconAsync);
    }

    private static async Task<ImageSource> DoGetIconAsync(string iconLink)
    {
      var imageSource = await WebImageDownloader.DownloadAsync(iconLink);
      return imageSource;
    }
  }
}