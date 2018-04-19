// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-04-18 6:56 PM

namespace eagleboost.shell.Tools
{
  using System;
  using System.Windows;
  using System.Windows.Interop;
  using System.Windows.Media.Imaging;

  public class ClipboardChangedEventArgs : EventArgs
  {
    public string Text { get; set; }

    public BitmapSource BitmapSource { get; set; }
  }

  public delegate void ClipboardChangedHandler(object sender, ClipboardChangedEventArgs args);

  public static class ClipboardMonitor
  {
    public static event ClipboardChangedHandler ClipboardUpdated;

    private static ClipboardReceiver Receiver
    {
      get { return Nested.Instance; }
    }

    public static void Install()
    {
      var receiver = Receiver;
      if (!receiver.IsVisible)
      {
        receiver.Show();
      }
    }

    public static void Uninstall()
    {
      var receiver = Receiver;
      receiver.Close();
    }

    private static void RaiseClipboardUpdated(ClipboardChangedEventArgs e)
    {
      var handler = ClipboardUpdated;
      if (handler != null)
      {
        handler(null, e);
      }
    }

    private class Nested
    {
      public static ClipboardReceiver Instance;

      static Nested()
      {
        Instance = new ClipboardReceiver { AllowsTransparency = true, ShowInTaskbar = false, WindowStyle = WindowStyle.None, Opacity = 0 };
      }
    }

    private class ClipboardReceiver : Window
    {
      private IntPtr _hWndNextViewer;
      private HwndSource _hWndSource;

      protected override void OnSourceInitialized(EventArgs e)
      {
        base.OnSourceInitialized(e);

        WindowInteropHelper wih = new WindowInteropHelper(this);
        _hWndSource = HwndSource.FromHwnd(wih.Handle);
        _hWndSource.AddHook(this.WinProc); // start processing window messages 
        _hWndNextViewer = Win32.SetClipboardViewer(_hWndSource.Handle); // set this window as a viewer 
      }

      protected override void OnClosed(EventArgs e)
      {
        base.OnClosed(e);

        _hWndSource.RemoveHook(this.WinProc);
      }

      private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
      {
        switch (msg)
        {
          case Win32.WM_CHANGECBCHAIN:
            if (wParam == _hWndNextViewer)
            {
              // Clipboard viewer chain changed, need to fix it. 
              _hWndNextViewer = lParam;
            }
            else if (_hWndNextViewer != IntPtr.Zero)
            {
              // pass the message to the next viewer. 
              Win32.SendMessage(_hWndNextViewer, msg, wParam, lParam);
            }
            break;

          case Win32.WM_DRAWCLIPBOARD:
            // Clipboard content changed
            OnClipboardUpdated();
            // pass the message to the next viewer. 
            Win32.SendMessage(_hWndNextViewer, msg, wParam, lParam);
            break;
        }

        return IntPtr.Zero;
      }

      private void OnClipboardUpdated()
      {
        var iData = Clipboard.GetDataObject();
        if (iData != null)
        {
          var args = new ClipboardChangedEventArgs();
          if (iData.GetDataPresent(DataFormats.Bitmap))
          {
            args.BitmapSource = iData.GetData(DataFormats.Bitmap) as BitmapSource;
          }

          if (iData.GetDataPresent(DataFormats.Text))
          {
            args.Text = iData.GetData(DataFormats.Text) as string;
          }

          RaiseClipboardUpdated(args);
        }
      }
    }
  }
}