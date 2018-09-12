// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-10 9:31 PM

namespace eagleboost.presentation.Behaviors
{
  using System;
  using System.Windows;
  using eagleboost.presentation.Win32;

  public static class HideMinMaxButton
  {
    #region Attached Properties
    #region HideMinimizeButton
    public static readonly DependencyProperty HideMinimizeButtonProperty = DependencyProperty.RegisterAttached(
      "HideMinimizeButton", typeof(bool), typeof(HideMinMaxButton), new PropertyMetadata(OnHideMinMaxButtonChanged));

    public static bool GetHideMinimizeButton(DependencyObject obj)
    {
      return (bool)obj.GetValue(HideMinimizeButtonProperty);
    }

    public static void SetHideMinimizeButton(DependencyObject obj, bool isEnabled)
    {
      obj.SetValue(HideMinimizeButtonProperty, isEnabled);
    }
    #endregion HideMinimizeButton

    #region HideMaximizeButton
    public static readonly DependencyProperty HideMaximizeButtonProperty = DependencyProperty.RegisterAttached(
      "HideMaximizeButton", typeof(bool), typeof(HideMinMaxButton), new PropertyMetadata(OnHideMinMaxButtonChanged));

    public static bool GetHideMaximizeButton(DependencyObject obj)
    {
      return (bool)obj.GetValue(HideMaximizeButtonProperty);
    }

    public static void SetHideMaximizeButton(DependencyObject obj, bool isEnabled)
    {
      obj.SetValue(HideMaximizeButtonProperty, isEnabled);
    }
    #endregion HideMaximizeButton

    private static void OnHideMinMaxButtonChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
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

      if (GetHideMinimizeButton(window))
      {
        NativeMethods.HideMinimizeButton(window);
      }

      if (GetHideMaximizeButton(window))
      {
        NativeMethods.HideMaximizeButton(window);
      }
    }
    #endregion Attached Properties  
  }
}