// Author : Shuo Zhang
// 
// Creation :2018-02-12 09:19

namespace eagleboost.presentation.Behaviors
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;
  using System.Windows.Interactivity;

  public abstract class ClearButtonBehavior<T> : Behavior<Button> where T : Control
  {
    #region Declarations
    private Button _button;
    #endregion Declarations

    #region Dependency Properties
    public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
      "Target", typeof(Control), typeof(ClearButtonBehavior<T>), new PropertyMetadata(default(Control)));

    public Control Target
    {
      get { return (Control) GetValue(TargetProperty); }
      set { SetValue(TargetProperty, value); }
    }
    #endregion Dependency Properties

    #region Protected Properties
    protected T Control
    {
      get { return (T) Target; }
    }
    #endregion Protected Properties

    #region Overrides
    protected override void OnAttached()
    {
      base.OnAttached();

      var button = AssociatedObject;
      button.PreviewMouseLeftButtonUp -= HandleButtonPreviewMouseLeftButtonUp;
      button.PreviewMouseLeftButtonUp += HandleButtonPreviewMouseLeftButtonUp;
      _button = button;
    }

    protected override void OnDetaching()
    {
      _button.PreviewMouseLeftButtonUp -= HandleButtonPreviewMouseLeftButtonUp;

      base.OnDetaching();
    }

    private void HandleButtonPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      OnButtonClick();
    }
    #endregion Overrides

    #region Virtuals
    protected abstract void OnButtonClick();
    #endregion Virtuals
  }
}