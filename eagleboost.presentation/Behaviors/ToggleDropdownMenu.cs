// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 23 9:08 PM

namespace eagleboost.presentation.Behaviors
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Controls.Primitives;
  using System.Windows.Data;
  using eagleboost.core.Utils;

  public class ToggleDropdownMenu
  {
    #region Attached Properties
    #region Menu
    public static readonly DependencyProperty MenuProperty = DependencyProperty.RegisterAttached(
      "Menu", typeof(ContextMenu), typeof(ToggleDropdownMenu), new PropertyMetadata(OnToggleDropdownMenuChanged));

    public static ContextMenu GetMenu(DependencyObject obj)
    {
      return (ContextMenu)obj.GetValue(MenuProperty);
    }

    public static void SetMenu(DependencyObject obj, ContextMenu menu)
    {
      obj.SetValue(MenuProperty, menu);
    }

    private static void OnToggleDropdownMenuChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      var menu = e.NewValue as ContextMenu;
      if (menu != null)
      {
        var toggleButton = ArgValidation.ThrowIfMismatch<ToggleButton>(obj, "obj");
        menu.PlacementTarget = toggleButton;
        menu.Placement = PlacementMode.Bottom;
        menu.SetBinding(ContextMenu.IsOpenProperty, new Binding(ToggleButton.IsCheckedProperty.Name) {Source = toggleButton, Mode = BindingMode.TwoWay});
      }
    }
    #endregion Menu
    #endregion Attached Properties  
  }
}