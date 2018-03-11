// Author : Shuo Zhang
// 
// Creation :2018-02-12 21:48

namespace eagleboost.presentation.Behaviors
{
  using System;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;
  using eagleboost.presentation.Extensions;

  public static class TextBoxSelectAll
  {
    #region Declarations
    private static bool _isAppDeactivated;
    #endregion Declarations

    public static void Install()
    {
      Application.Current.Deactivated += HandleAppDeactivated;
      EventManager.RegisterClassHandler(typeof(TextBox), UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(HandlePreviewMouseLeftButtonDown));
      EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(HandleGotKeyboardFocus));
      EventManager.RegisterClassHandler(typeof(TextBox), Control.MouseDoubleClickEvent, new MouseButtonEventHandler(HandleDoubleClick));
      EventManager.RegisterClassHandler(typeof(TextBox), UIElement.KeyDownEvent, new KeyEventHandler(HandleKeyDown));
      EventManager.RegisterClassHandler(typeof(TextBox), UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(HandleMouseLeftButtonUp), true);
    }

    private static void HandleAppDeactivated(object sender, EventArgs e)
    {
      _isAppDeactivated = true;
    }

    private static void HandlePreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var textBox = ((DependencyObject) e.OriginalSource).FindParent<TextBox>();
      if (textBox != null)
      {
        if (!textBox.IsKeyboardFocusWithin || _isAppDeactivated)
        {
          textBox.Focus();
          e.Handled = true;
          _isAppDeactivated = false;
        }
      }
    }

    private static void HandleGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
      var textBox = ((DependencyObject)e.OriginalSource).FindParent<TextBox>();
      if (textBox != null)
      {
        textBox.SelectAll();
      }
    }

    private static void HandleDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var textBox = ((DependencyObject)e.OriginalSource).FindParent<TextBox>();
      if (textBox != null)
      {
        textBox.SelectAll();
      }
    }

    private static void HandleMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      var textBox = ((DependencyObject)e.OriginalSource).FindParent<TextBox>();
      if (textBox != null)
      {
        textBox.SelectAll();
      }
    }

    private static void HandleKeyDown(object sender, KeyEventArgs e)
    {
      var textBox = ((DependencyObject)e.OriginalSource).FindParent<TextBox>();
      if (textBox != null)
      {
        textBox.SelectAll();
      }
    }
  }
}