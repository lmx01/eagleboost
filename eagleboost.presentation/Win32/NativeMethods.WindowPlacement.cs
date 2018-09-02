// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-01 8:36 PM

namespace eagleboost.presentation.Win32
{
  using System;
  using System.Runtime.InteropServices;
  using System.Windows;
  using System.Windows.Interop;

  /// <summary>
  /// https://blogs.msdn.microsoft.com/davidrickard/2010/03/08/saving-window-size-and-location-in-wpf-and-winforms/
  /// </summary>
  public partial class NativeMethods
  {
    // RECT structure required by WINDOWPLACEMENT structure
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
      public int Left;
      public int Top;
      public int Right;
      public int Bottom;

      public RECT(int left, int top, int right, int bottom)
      {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
      }
    }

    // POINT structure required by WINDOWPLACEMENT structure
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
      public int X;
      public int Y;

      public POINT(int x, int y)
      {
        X = x;
        Y = y;
      }
    }

    // WINDOWPLACEMENT stores the position, size, and state of a window
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPLACEMENT
    {
      public int length;
      public int flags;
      public int showCmd;
      public POINT minPosition;
      public POINT maxPosition;
      public RECT normalPosition;
    }

    public static void SetPlacement(Window window, WINDOWPLACEMENT placement)
    {
      var hwnd = new WindowInteropHelper(window).Handle;
      placement.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
      placement.flags = 0;
      placement.showCmd = placement.showCmd == SW_SHOWMINIMIZED ? SW_SHOWNORMAL : placement.showCmd;
      SetWindowPlacement(hwnd, ref placement);
    }

    public static WINDOWPLACEMENT GetPlacement(Window window)
    {
      var hwnd = new WindowInteropHelper(window).Handle;
      WINDOWPLACEMENT placement;
      GetWindowPlacement(hwnd, out placement);
      return placement;
    }
  }
}