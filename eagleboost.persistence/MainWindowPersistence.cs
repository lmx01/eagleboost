// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-03-11 10:16 PM

namespace eagleboost.persistence
{
  using System.Windows;

  public class MainWindowPersistence
  {
    #region Declarations
    private readonly string _appName;
    #endregion Declarations

    #region ctors
    public MainWindowPersistence(string appName = null)
    {
      _appName = appName ?? "Default";
      Application.Current.Startup += HandleApplicationStartup;
      Application.Current.Exit += HandleApplicationExit;
      Application.Current.MainWindow.Closed += HandleMainWindowClosed;
    }

    private void HandleMainWindowClosed(object sender, System.EventArgs e)
    {
      throw new System.NotImplementedException();
    }

    private void HandleApplicationExit(object sender, ExitEventArgs e)
    {
      throw new System.NotImplementedException();
    }

    private void HandleApplicationStartup(object sender, StartupEventArgs e)
    {
      throw new System.NotImplementedException();
    }
    #endregion ctors
  }
}