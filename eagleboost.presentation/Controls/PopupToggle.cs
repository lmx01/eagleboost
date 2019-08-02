// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 25 8:35 PM

namespace eagleboost.presentation.Controls
{
  using System;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Controls.Primitives;
  using System.Windows.Input;
  using eagleboost.presentation.Extensions;

  /// <summary>
  /// PopupToggle
  /// </summary>
  public class PopupToggle : Control
  {
    #region Statics
    private const string PopupRoot = "System.Windows.Controls.Primitives.PopupRoot";
    #endregion Statics

    #region Declarations
    private Action _hookWindow;
    private Action _unhookWindow;
    #endregion Declarations

    #region ctors
    static PopupToggle()
    {
      FrameworkElementExt.OverrideDefaultStyleKey();
      EventManager.RegisterClassHandler(typeof(PopupToggle), Mouse.MouseDownEvent, new MouseButtonEventHandler(OnMouseButtonDown), true);
    }
    #endregion ctors

    #region Dependency Properties
    #region IsChecked
    public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
      "IsChecked", typeof(bool), typeof(PopupToggle), new PropertyMetadata(OnIsCheckedChanged));

    public bool IsChecked
    {
      get { return (bool) GetValue(IsCheckedProperty); }
      set { SetValue(IsCheckedProperty, value); }
    }

    private static void OnIsCheckedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((PopupToggle) obj).OnIsCheckedChanged();
    }    
    #endregion IsChecked

    #region Content
    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
      "Content", typeof(object), typeof(PopupToggle));

    public object Content
    {
      get { return GetValue(ContentProperty); }
      set { SetValue(ContentProperty, value); }
    }
    #endregion Content
    #endregion Dependency Properties

    #region Private Properties
    private ToggleButton ToggleButton
    {
      get { return (ToggleButton)GetTemplateChild("PART_ToggleButton"); }
    }
    #endregion Private Properties
    
    #region Event Handlers
    private static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
    {
      var tb = (PopupToggle)sender;
      if (!tb.IsKeyboardFocusWithin)
      {
        tb.Focus();
      }

      e.Handled = true;
      if (Mouse.Captured != sender || e.OriginalSource != sender)
      {
        return;
      }

      Close(tb.ToggleButton);
    }

    private void OnIsCheckedChanged()
    {
      var isChecked = IsChecked;
      if (isChecked)
      {
        OnChecked();
      }
      else
      {
        OnUnchecked();
      }
    }

    private void HandleWindowPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      if (!IsChecked || IsMouseOver)
      {
        return;
      }

      var element = e.OriginalSource as DependencyObject;
      CloseIfNotPopup(element);
    }
    #endregion Event Handlers

    #region Private Methods
    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();

      var window = this.FindParent<Window>();
      if (window != null)
      {
        _hookWindow = () => window.PreviewMouseDown += HandleWindowPreviewMouseDown;
        _unhookWindow = () => window.PreviewMouseDown -= HandleWindowPreviewMouseDown;
      }

      var menu = ContextMenuService.GetContextMenu(this);
      if (menu != null)
      {
        menu.Closed += HandleMenuClosed;
      }
    }

    private void OnChecked()
    {
      if (_hookWindow != null)
      {
        _hookWindow();
      }
    }

    private void OnUnchecked()
    {
      if (_unhookWindow != null)
      {
        _unhookWindow();
      }
    }

    private void CloseIfNotPopup(DependencyObject element)
    {
      if (element != null)
      {
        if (element.GetType().FullName != PopupRoot)
        {
          var popup = element.FindParent<FrameworkElement>(o => o.GetType().FullName == PopupRoot);
          if (popup == null)
          {
            Close(ToggleButton);
          }
        }
      }
    }
    #endregion Private Methods

    #region Event Handlers
    private void HandleMenuClosed(object sender, RoutedEventArgs e)
    {
      SetCurrentValue(IsCheckedProperty, false);
    }
    #endregion Event Handlers

    #region Overrides
    protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
    {
      base.OnIsKeyboardFocusWithinChanged(e);

      if (IsChecked && !IsKeyboardFocusWithin)
      {
        var focusedElement = Keyboard.FocusedElement as DependencyObject;
        if (focusedElement == null)
        {
          Close(ToggleButton);
        }
        else
        {
          CloseIfNotPopup(focusedElement);
        }
      }
    }
    #endregion Overrides

    #region Private Methods
    private static void Close(ToggleButton tb)
    {
      tb.IsChecked = false;
    }
    #endregion Private Methods
  }
}