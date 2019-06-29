// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 12:47 PM

namespace eagleboost.presentation.Behaviors
{
  using System.Windows;
  using System.Windows.Controls.Primitives;
  using eagleboost.core.Collections;

  /// <summary>
  /// SelectionContainerToggleButton
  /// </summary>
  public class SelectionContainerToggleButton : SelectionContainerToggleButtonState
  {
    #region Overrides
    protected override void OnAttached()
    {
      base.OnAttached();

      var toggleButton = AssociatedObject;
      toggleButton.Checked += HandledChecked;
      toggleButton.Unchecked += HandleUnchecked;
    }

    protected override void OnDetaching()
    {
      var toggleButton = AssociatedObject;
      if (toggleButton != null)
      {
        toggleButton.Checked -= HandledChecked;
        toggleButton.Unchecked += HandleUnchecked;
      }

      base.OnDetaching();
    }
    #endregion Overrides

    #region Event Handlers
    private void HandleUnchecked(object sender, RoutedEventArgs e)
    {
      var c = SelectionContainer;
      var b = (ToggleButton) sender;
      c.Unselect(DataItem ?? b.DataContext);
    }

    private void HandledChecked(object sender, RoutedEventArgs e)
    {
      var c = SelectionContainer;
      var b = (ToggleButton)sender;
      c.Select(DataItem ?? b.DataContext);
    }
    #endregion Event Handlers
  }
}