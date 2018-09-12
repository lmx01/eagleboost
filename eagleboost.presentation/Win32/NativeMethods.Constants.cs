// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-10 9:33 PM

namespace eagleboost.presentation.Win32
{
  public partial class NativeMethods
  {
    internal const int GWL_EXSTYLE = -20;
    internal const int WS_EX_DLGMODALFRAME = 0x0001;
    internal const int SWP_NOSIZE = 0x0001;
    internal const int SWP_NOMOVE = 0x0002;
    internal const int SWP_NOZORDER = 0x0004;
    internal const int SWP_FRAMECHANGED = 0x0020;
    internal const uint WM_SETICON = 0x0080;
    internal const int GWL_STYLE = -16;
    internal const int WS_MAXIMIZEBOX = 0x10000;
    internal const int WS_MINIMIZEBOX = 0x20000;
    internal const int SW_SHOWNORMAL = 1;
    internal const int SW_SHOWMINIMIZED = 2;
  }
}