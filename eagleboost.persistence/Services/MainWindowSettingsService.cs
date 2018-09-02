// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-29 1:17 PM

namespace eagleboost.persistence.Services
{
  using System;
  using System.ComponentModel;
  using System.Windows;
  using eagleboost.persistence.Contracts;

  /// <summary>
  /// MainWindowSettingsService
  /// </summary>
  public partial class MainWindowSettingsService : IMainWindowSettingsService
  {
    #region Declarations
    private readonly ISettingsService _settingsService;
    #endregion Declarations

    #region ctors
    public MainWindowSettingsService(ISettingsService settingsService)
    {
      _settingsService = settingsService;
      var mainWindow = Application.Current.MainWindow;
      if (mainWindow == null)
      {
        throw new ApplicationException("Main window is not ready yet");
      }

      mainWindow.SourceInitialized += HandleMainWindowSourceInitialized;
      mainWindow.Closing += HandleMainWindowClosing;
    }
    #endregion ctors

    #region Event Handlers
    private void HandleMainWindowSourceInitialized(object sender, EventArgs e)
    {
      var window = (Window)sender;
      window.SourceInitialized -= HandleMainWindowSourceInitialized;

      LoadState(window);
    }

    private void HandleMainWindowClosing(object sender, CancelEventArgs e)
    {
      var window = (Window)sender;
      window.Closing -= HandleMainWindowClosing;

      SaveState(window);
    }
    #endregion Event Handlers

    #region Private Methods
    private void SaveState(Window window)
    {
      var state = new MainWindowState
      {
        WindowState = window.WindowState,
        Top = window.Top,
        Left = window.Left,
        Width = window.Width,
        Height = window.Height
      };

      _settingsService.Save("MainWindowState", state);
    }

    private void LoadState(Window window)
    {
      var state = _settingsService.Load<MainWindowState>("MainWindowState");
      if (state != null)
      {
        window.Top = state.Top;
        window.Left = state.Left;
        window.Width = state.Width;
        window.Height = state.Height;
        window.WindowStartupLocation = WindowStartupLocation.Manual;
        window.WindowState = state.WindowState;
      }
      else
      {
        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
      }
    }
    #endregion Private Methods
  }
}