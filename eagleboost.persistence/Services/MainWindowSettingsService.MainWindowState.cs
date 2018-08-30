// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-28 2:47 PM

namespace eagleboost.persistence.Services
{
  using System.Windows;

  /// <summary>
  /// MainWindowSettingsService
  /// </summary>
  public partial class MainWindowSettingsService
  {
    private class MainWindowState
    {
      public WindowState WindowState { get; set; }
      public double Top { get; set; }
      public double Left { get; set; }
      public double Width { get; set; }
      public double Height { get; set; }
    }
  }

}