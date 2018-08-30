// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-29 1:17 PM

namespace eagleboost.persistence.Services
{
  using System;
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

      LoadState(mainWindow);
      mainWindow.Closed += HandleMainWindowClosed;
    }
    #endregion ctors

    #region Event Handlers
    private void HandleMainWindowClosed(object sender, EventArgs e)
    {
      var window = (Window)sender;
      window.Closed -= HandleMainWindowClosed;

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
        window.WindowState = state.WindowState;
        window.Top = state.Top;
        window.Left = state.Left;
        window.Width = state.Width;
        window.Height = state.Height;
      }
    }
    #endregion Private Methods
  }
}