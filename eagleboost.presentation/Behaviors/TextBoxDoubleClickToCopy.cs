namespace eagleboost.presentation.Behaviors
{
  using System;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;
  using eagleboost.core.Data;
  using eagleboost.core.Extensions;

  /// <summary>
  /// TextBoxDoubleClickToCopy
  /// </summary>
  public class TextBoxDoubleClickToCopy
  {
    #region IsEnabled
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
      "IsEnabled", typeof(bool), typeof(TextBoxDoubleClickToCopy), new PropertyMetadata(OnIsEnabledChanged));

    public static bool GetIsEnabled(DependencyObject obj)
    {
      return (bool)obj.GetValue(IsEnabledProperty);
    }

    public static void SetIsEnabled(DependencyObject obj, bool value)
    {
      obj.SetValue(IsEnabledProperty, BooleanBoxes.Box(value));
    }

    private static void OnIsEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      var t = obj as TextBox;
      if (t == null)
      {
        throw new ArgumentException(obj + @" is not TextBox", @"obj");
      }

      if ((bool)e.NewValue)
      {
        t.MouseDoubleClick += HandleTextMouseDoubleClick;
      }
      else
      {
        t.MouseDoubleClick -= HandleTextMouseDoubleClick;
      }
    }
    #endregion IsEnabled

    #region Event Handlers
    private static void HandleTextMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var t = (TextBox) sender;
      var text = t.Text;
      if (text.HasValue())
      {
        Clipboard.SetText(text);
      }
    }
    #endregion Event Handlers
  }
}