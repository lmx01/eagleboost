// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-01 8:46 PM

namespace eagleboost.presentation.Extensions
{
  using System.Windows;
  using eagleboost.presentation.Behaviors;

  public static class WindowExt
  {
    public static Window RemoveIcon(this Window window)
    {
      RemoveWindowIcon.SetRemoveIcon(window, true);
      return window;
    }
  }
}