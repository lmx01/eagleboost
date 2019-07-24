using System.Windows;

namespace eagleboost.sampleapp
{
  using eagleboost.UserExperience.Threading;

  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      UiThread.Initialize();
    }
  }
}
