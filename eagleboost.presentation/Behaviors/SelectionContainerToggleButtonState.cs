// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 4:08 PM

namespace eagleboost.presentation.Behaviors
{
  using System.Windows;
  using System.Windows.Controls.Primitives;
  using System.Windows.Data;
  using System.Windows.Interactivity;
  using eagleboost.core.Collections;

  /// <summary>
  /// SelectionContainerToggleButtonState
  /// </summary>
  public class SelectionContainerToggleButtonState : Behavior<ToggleButton>
  {
    #region Dependency Properties
    #region SelectionContainer
    public static readonly DependencyProperty SelectionContainerProperty = DependencyProperty.Register(
      "SelectionContainer", typeof(ISelectionContainer), typeof(SelectionContainerToggleButtonState), new PropertyMetadata(OnSelectionContainerChanged));

    public ISelectionContainer SelectionContainer
    {
      get { return (ISelectionContainer)GetValue(SelectionContainerProperty); }
      set { SetValue(SelectionContainerProperty, value); }
    }

    private static void OnSelectionContainerChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((SelectionContainerToggleButtonState)obj).UpdateIsCheckedBinding();
    }
    #endregion SelectionContainer

    #region DataItem
    public static readonly DependencyProperty DataItemProperty = DependencyProperty.Register(
      "DataItem", typeof(object), typeof(SelectionContainerToggleButtonState), new PropertyMetadata(OnDataItemChanged));

    public object DataItem
    {
      get { return GetValue(DataItemProperty); }
      set { SetValue(DataItemProperty, value); }
    }

    private static void OnDataItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((SelectionContainerToggleButtonState)obj).UpdateIsCheckedBinding();
    }
    #endregion DataItem
    #endregion Dependency Properties

    #region Event Handlers
    private void UpdateIsCheckedBinding()
    {
      var dataItem = DataItem;
      var c = SelectionContainer;
      var toggleButton = AssociatedObject;
      if (dataItem == null || c == null || toggleButton == null)
      {
        return;
      }

      var path = new PropertyPath("[(0)]", dataItem);
      var binding = new Binding { Path = path, Source = c, Mode = BindingMode.OneWay };
      toggleButton.SetBinding(ToggleButton.IsCheckedProperty, binding);
    }
    #endregion Event Handlers
  }
}