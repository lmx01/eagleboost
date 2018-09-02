// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-01 8:41 PM

namespace eagleboost.presentation.Win32
{
  using System;
  using System.Runtime.InteropServices;

  public partial class NativeMethods
  {
    [DllImport("user32.dll")]
    static extern int GetWindowLong(IntPtr hwnd, int index);

    [DllImport("user32.dll")]
    static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
      int x, int y, int width, int height, uint flags);

    [DllImport("user32.dll")]
    static extern IntPtr SendMessage(IntPtr hwnd, uint msg,
      IntPtr wParam, IntPtr lParam);

    const int GWL_EXSTYLE = -20;
    const int WS_EX_DLGMODALFRAME = 0x0001;
    const int SWP_NOSIZE = 0x0001;
    const int SWP_NOMOVE = 0x0002;
    const int SWP_NOZORDER = 0x0004;
    const int SWP_FRAMECHANGED = 0x0020;
    const uint WM_SETICON = 0x0080;
  }
}