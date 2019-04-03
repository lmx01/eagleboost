// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 01 10:51 PM

namespace eagleboost.shell.Tools
{
  using System;
  using System.ComponentModel;
  using System.Diagnostics;
  using System.Threading;
  using System.Runtime.InteropServices;
  using System.Windows;
  using System.Windows.Interop;

  /// <summary>
  /// ApplicationActivationTracker
  /// </summary>
  public partial class ApplicationActivationTracker
  {
    /// <summary>
    /// ActivationTracker
    /// </summary>
    private static class ActivationTracker
    {
      #region Consts
      private const int WINEVENT_INCONTEXT = 4;
      private const int WINEVENT_OUTOFCONTEXT = 0;
      private const int WINEVENT_SKIPOWNPROCESS = 2;
      private const int WINEVENT_SKIPOWNTHREAD = 1;
      private const int EVENT_SYSTEM_FOREGROUND = 3;
      #endregion Consts

      #region DllImports
      [DllImport("user32.dll", SetLastError = true)]
      private static extern IntPtr SetWinEventHook(int eventMin, int eventMax, IntPtr winEventProc, WinEventProc winEventCallback, int idProcess, int idThread, int dwflags);

      [DllImport("user32.dll", SetLastError = true)]
      private static extern int UnhookWinEvent(IntPtr hWinEventHook);

      [DllImport("user32.dll", SetLastError = true)]
      private static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
      #endregion DllImports

      #region Declarations
      private delegate void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
      private static int? _currentProcessId;
      private static IntPtr _winEventHook;
      private static WinEventProc _winEventHookCallback;
      private static int _count;
      private static uint? _prevProcessId;
      #endregion Declarations

      #region Public Properties
      public static int CurrentProcessId
      {
        get
        {
          if (!_currentProcessId.HasValue)
          {
            _currentProcessId = Process.GetCurrentProcess().Id;
          }

          return _currentProcessId.Value;
        }
      }
      #endregion Public Properties

      #region Public Methods
      public static void Start()
      {
        if (Interlocked.Increment(ref _count) == 1)
        {
          if (_winEventHook == IntPtr.Zero)
          {
            _winEventHookCallback = WinEventHookCallback;
            _winEventHook = SetWinEventHook(
              EVENT_SYSTEM_FOREGROUND, // eventMin
              EVENT_SYSTEM_FOREGROUND, // eventMax
              IntPtr.Zero, // winEventProc
              _winEventHookCallback, // winEventCallback
              0, // idProcess
              0, // idThread
              WINEVENT_OUTOFCONTEXT);

            if (_winEventHook == IntPtr.Zero)
            {
              throw new Win32Exception(Marshal.GetLastWin32Error());
            }
          }
        }
      }

      public static void Stop()
      {
        if (Interlocked.Decrement(ref _count) == 0)
        {
          if (_winEventHook != IntPtr.Zero)
          {
            UnhookWinEvent(_winEventHook);
          }
        }
      }
      #endregion Public Methods

      #region Event Handlers
      private static void WinEventHookCallback(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
      {
        uint processId;
        GetWindowThreadProcessId(hwnd, out processId);
        if (processId == CurrentProcessId)
        {
          if (!_prevProcessId.HasValue)
          {
            _prevProcessId = processId;
            var hwndSource = HwndSource.FromHwnd(hwnd);
            if (hwndSource != null)
            {
              var window = (Window) hwndSource.RootVisual;
              RaiseActivated(window);
            }
            else
            {
              RaiseActivated(null);
            }
          }
        }
        else
        {
          if (_prevProcessId == CurrentProcessId)
          {
            _prevProcessId = null;
            RaiseDeactivated();
          }
        }
      }
      #endregion Event Handlers

      #region Events
      public static event EventHandler Activated;

      private static void RaiseActivated(Window window)
      {
        var handler = Activated;
        if (handler != null)
        {
          handler(window, EventArgs.Empty);
        }
      }

      public static event EventHandler Deactivated;

      private static void RaiseDeactivated()
      {
        var handler = Deactivated;
        if (handler != null)
        {
          handler(null, EventArgs.Empty);
        }
      }
      #endregion Events
    }
  }
}