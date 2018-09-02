// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-01 8:42 PM

namespace eagleboost.presentation.Win32
{
  using System;
  using System.Windows;
  using System.Windows.Interop;

  public partial class NativeMethods
  {
    public static void RemoveIcon(Window window)
    {
      // Get this window's handle
      IntPtr hwnd = new WindowInteropHelper(window).Handle;

      // Change the extended window style to not show a window icon
      int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
      SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);

      // Update the window's non-client area to reflect the changes
      SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
    }
  }
}