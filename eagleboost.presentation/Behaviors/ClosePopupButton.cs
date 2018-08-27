// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-27 12:50 AM

namespace eagleboost.presentation.Behaviors
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Controls.Primitives;
  using System.Windows.Interactivity;
  using eagleboost.presentation.Extensions;

  /// <summary>
  /// ClosePopupButton
  /// </summary>
  public class ClosePopupButton : Behavior<Button>
  {
    #region Declarations
    private Popup _popup;
    #endregion Declarations

    #region Overrides
    protected override void OnAttached()
    {
      base.OnAttached();

      var button = AssociatedObject;
      var popup = _popup=button.FindParent<Popup>();
      if (popup != null && popup.StaysOpen)
      {
        button.Click += HandleClick;
      }
    }

    protected override void OnDetaching()
    {
      var button = AssociatedObject;
      if (button != null)
      {
        button.Click -= HandleClick;
      }

      base.OnDetaching();
    }
    #endregion Overrides

    #region Event Handlers
    private void HandleClick(object sender, RoutedEventArgs e)
    {
      _popup.IsOpen = false;
    }
    #endregion Event Handlers
  }
}