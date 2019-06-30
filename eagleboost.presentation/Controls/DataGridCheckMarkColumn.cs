// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 30 11:48 AM

namespace eagleboost.presentation.Controls
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Controls.Primitives;

  /// <summary>
  /// DataGridCheckMarkColumn
  /// </summary>
  public class DataGridCheckMarkColumn : DataGridCheckBoxColumn
  {
    #region Overrides
    protected override FrameworkElement GenerateElement(DataGridCell cell,object dataItem)
    {
      return GenerateCheckBox(false, cell);
    }

    protected override FrameworkElement GenerateEditingElement(DataGridCell cell,object dataItem)
    {
      return GenerateCheckBox(true, cell);
    }
    #endregion Overrides

    #region Private Methods
    private void ApplyStyle(bool isEditing, bool defaultToElementStyle, FrameworkElement element)
    {
      var style = PickStyle(isEditing, defaultToElementStyle);
      if (style != null)
      {
        element.Style = style;
      }
    }

    private Style PickStyle(bool isEditing, bool defaultToElementStyle)
    {
      var style = isEditing ? EditingElementStyle : ElementStyle;
      if (isEditing & defaultToElementStyle && style == null)
      {
        style = ElementStyle;
      }

      return style;
    }

    private CheckBox GenerateCheckBox(bool isEditing, DataGridCell cell)
    {
      var checkBox = (cell != null ? cell.Content as CheckBox : null) ?? new CheckBox{Template = (ControlTemplate)cell.TryFindResource("CheckMarkControlTemplate") };
      checkBox.IsThreeState = IsThreeState;
      ApplyStyle(isEditing, true, checkBox);
      checkBox.SetBinding(ToggleButton.IsCheckedProperty, Binding);
      return checkBox;
    }
    #endregion Private Methods
  }
}