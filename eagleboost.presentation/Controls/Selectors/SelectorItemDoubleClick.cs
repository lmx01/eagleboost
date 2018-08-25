// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-25 12:20 AM

namespace eagleboost.presentation.Controls.Selectors
{
  using System.Windows;
  using System.Windows.Controls.Primitives;
  using System.Windows.Input;
  using System.Windows.Interactivity;
  using eagleboost.presentation.Contracts;
  using eagleboost.presentation.Extensions;

  public class SelectorItemDoubleClick : Behavior<Selector>
  {
    #region Dependency Properties
    public static readonly DependencyProperty SelectItemReceiverProperty = DependencyProperty.Register(
      "SelectItemReceiver", typeof(ISelectItemReceiver), typeof(SelectorItemDoubleClick));

    public ISelectItemReceiver SelectItemReceiver
    {
      get { return (ISelectItemReceiver) GetValue(SelectItemReceiverProperty); }
      set { SetValue(SelectItemReceiverProperty, value); }
    }
    #endregion Dependency Properties

    #region Overrides
    protected override void OnAttached()
    {
      base.OnAttached();

      var selector = AssociatedObject;
      selector.MouseDoubleClick += HandleMouseDoubleClick;
    }

    protected override void OnDetaching()
    {
      var selector = AssociatedObject;
      selector.MouseDoubleClick -= HandleMouseDoubleClick;

      base.OnDetaching();
    }
    #endregion Overrides

    #region Event Handlers
    private void HandleMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var selector = (Selector) sender;
      var selectedItem = selector.SelectedItem;
      if (selectedItem != null)
      {
        var container = selector.ItemContainerGenerator.ContainerFromItem(selectedItem);
        if (container != null)
        {
          SelectItemReceiver.SelectItemCommand.TryExecute(selectedItem);
        }
      }
    }
    #endregion Event Handlers
  }
}