// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2019-07-12 21:03

namespace eagleboost.presentation.Behaviors
{
  using System.Windows;
  using System.Windows.Controls.Primitives;
  using System.Windows.Interactivity;

  /// <summary>
  /// PopupBottomPlacement
  /// </summary>
  public class PopupBottomPlacement : Behavior<Popup>
  {
    #region Overrides
    protected override void OnAttached()
    {
      base.OnAttached();

      var popup = AssociatedObject;
      popup.Placement = PlacementMode.Custom;
      popup.CustomPopupPlacementCallback += HandleCustomPopupPlacement;
    }
    #endregion Overrides

    #region Overrides
    private CustomPopupPlacement[] HandleCustomPopupPlacement(Size popupSize, Size targetSize, Point p)
    {
      var element = (FrameworkElement)AssociatedObject.PlacementTarget;
      p.Y += element.ActualHeight + 10;
      return new CustomPopupPlacement[] {new CustomPopupPlacement(p, PopupPrimaryAxis.Horizontal)};
    }
    #endregion Overrides
  }
}