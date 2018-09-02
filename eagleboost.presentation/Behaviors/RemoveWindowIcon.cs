// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-01 8:36 PM

namespace eagleboost.presentation.Behaviors
{
  using System;
  using System.Windows;
  using eagleboost.presentation.Win32;

  public static class RemoveWindowIcon
  {
    #region Attached Properties
    public static readonly DependencyProperty RemoveIconProperty = DependencyProperty.RegisterAttached(
      "RemoveIcon", typeof(bool), typeof(RemoveWindowIcon), new PropertyMetadata(OnRemoveIconChanged));

    public static bool GetRemoveIcon(DependencyObject obj)
    {
      return (bool)obj.GetValue(RemoveIconProperty);
    }

    public static void SetRemoveIcon(DependencyObject obj, bool isEnabled)
    {
      obj.SetValue(RemoveIconProperty, isEnabled);
    }

    private static void OnRemoveIconChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      var window = (Window)obj;
      window.SourceInitialized -= HandleWindowSourceInitialized;
      if ((bool)e.NewValue)
      {
        window.SourceInitialized += HandleWindowSourceInitialized;
      }
    }

    private static void HandleWindowSourceInitialized(object sender, EventArgs e)
    {
      var window = (Window)sender;
      window.SourceInitialized -= HandleWindowSourceInitialized;
      NativeMethods.RemoveIcon(window);
    }
    #endregion Attached Properties  
  }
}