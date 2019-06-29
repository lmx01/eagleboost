// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 3:44 PM

namespace eagleboost.presentation.Controls.Buttons
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;

  /// <summary>
  /// AutoInvalidateButton - Auto update enable state when CommandParameter is changed
  /// </summary>
  public class AutoInvalidateButton : Button
  {
    #region Declarations
    private readonly CommandHelpers<AutoInvalidateButton> _helpers;
    #endregion Declarations

    #region ctors
    static AutoInvalidateButton()
    {
      CommandProperty.OverrideMetadata(typeof(AutoInvalidateButton), new FrameworkPropertyMetadata(OnCommandChanged));
      CommandParameterProperty.OverrideMetadata(typeof(AutoInvalidateButton), new FrameworkPropertyMetadata(OnCommandParameterChanged));
    }

    public AutoInvalidateButton()
    {
      _helpers = new CommandHelpers<AutoInvalidateButton>(this);
    }
    #endregion ctors

    #region Private Methods
    private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((AutoInvalidateButton)d)._helpers.OnCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue);
    }

    private static void OnCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((AutoInvalidateButton)d)._helpers.OnCommandParameterChanged(e.OldValue, e.NewValue);
    }
    #endregion Private Methods

    #region Overrides
    protected override bool IsEnabledCore
    {
      get { return _helpers.CanExecute; }
    }
    #endregion Overrides
  }
}