// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-29 7:25 PM

namespace eagleboost.presentation.Controls
{
  using System.Windows;
  using System.Windows.Interactivity;
  using eagleboost.presentation.Interactivity;

  public class ViewControllerWindow : Window
  {
    #region ctors
    public ViewControllerWindow()
    {
      var behaviors = Interaction.GetBehaviors(this);
      behaviors.Add(new ViewControllerBehavior());
      WindowStyle = WindowStyle.SingleBorderWindow;
      ResizeMode = ResizeMode.NoResize;
      SizeToContent = SizeToContent.WidthAndHeight;
      ShowInTaskbar = false;
      Owner = Application.Current.MainWindow;
      WindowStartupLocation = WindowStartupLocation.CenterOwner;
    }
    #endregion ctors
  }
}