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
    internal static extern int GetWindowLong(IntPtr hwnd, int index);

    [DllImport("user32.dll")]
    internal static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

    [DllImport("user32.dll")]
    internal static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);

    [DllImport("user32.dll")]
    internal static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    internal static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

    [DllImport("user32.dll")]
    internal static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

  }
}