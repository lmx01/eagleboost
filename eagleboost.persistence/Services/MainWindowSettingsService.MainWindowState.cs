// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-28 2:47 PM

namespace eagleboost.persistence.Services
{
  using eagleboost.presentation.Win32;

  /// <summary>
  /// MainWindowSettingsService
  /// </summary>
  public partial class MainWindowSettingsService
  {
    private class MainWindowState
    {
      public NativeMethods.WINDOWPLACEMENT Placement { get; set; }
    }
  }
}