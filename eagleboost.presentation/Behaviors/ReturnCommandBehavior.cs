// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 14 3:35 PM

namespace eagleboost.presentation.Behaviors
{
  using System.Windows;
  using System.Windows.Input;
  using System.Windows.Interactivity;
  using eagleboost.presentation.Extensions;

  /// <summary>
  /// ReturnCommandBehavior
  /// </summary>
  public class ReturnCommandBehavior : Behavior<FrameworkElement>
  {
    #region Dependency Properties
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
      "Command", typeof(ICommand), typeof(ReturnCommandBehavior));

    public ICommand Command
    {
      get { return (ICommand) GetValue(CommandProperty); }
      set { SetValue(CommandProperty, value); }
    }

    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
      "CommandParameter", typeof(object), typeof(ReturnCommandBehavior));

    public object CommandParameter
    {
      get { return GetValue(CommandParameterProperty); }
      set { SetValue(CommandParameterProperty, value); }
    }
    #endregion Dependency Properties

    #region Overrides
    protected override void OnAttached()
    {
      base.OnAttached();

      var element = AssociatedObject;
      element.PreviewKeyUp += HandleElementPreviewKeyUp;
    }
    #endregion Overrides

    #region Event Handlers
    private void HandleElementPreviewKeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
      {
        Command.TryExecute(CommandParameter);
      }
    }
    #endregion Event Handlers
  }
}