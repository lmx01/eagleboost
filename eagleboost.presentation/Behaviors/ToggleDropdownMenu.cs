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
    public static readonly DependencyProperty IsOpenPropertyProperty = DependencyProperty.RegisterAttached(
      "IsOpenProperty", typeof(DependencyProperty), typeof(ToggleDropdownMenu), new PropertyMetadata(OnIsOpenPropertyChanged));

    public static DependencyProperty GetIsOpenProperty(DependencyObject obj)
    {
      return (DependencyProperty)obj.GetValue(IsOpenPropertyProperty);
    }

    public static void SetIsOpenProperty(DependencyObject obj, DependencyProperty menu)
    {
      obj.SetValue(IsOpenPropertyProperty, menu);
    }

    private static void OnIsOpenPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      TrySetupMenu(obj);
    }
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
      TrySetupMenu(obj);
    }
    #endregion Menu
    #endregion Attached Properties

    #region Private Methods

    private static void TrySetupMenu(DependencyObject obj)
    {
      var property = GetIsOpenProperty(obj);
      var menu = GetMenu(obj);
      if (menu != null && property != null)
      {
        var toggle = ArgValidation.ThrowIfMismatch<FrameworkElement>(obj, "obj");
        menu.PlacementTarget = toggle;
        menu.Placement = PlacementMode.Bottom;
        menu.SetBinding(ContextMenu.IsOpenProperty, new Binding(property.Name) { Source = toggle, Mode = BindingMode.OneWay });
        ContextMenuService.SetContextMenu(toggle, menu);
      }
    }
    #endregion Private Methods
  }
}