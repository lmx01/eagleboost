// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 12:03 PM

namespace eagleboost.presentation.Behaviors
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Controls.Primitives;
  using System.Windows.Interactivity;
  using eagleboost.core.Collections;

  /// <summary>
  /// SelectionContainerBehavior
  /// </summary>
  public class SelectionContainerBehavior : Behavior<Selector>
  {
    #region Dependency Properties
    #region SelectionContainer
    public static readonly DependencyProperty SelectionContainerProperty = DependencyProperty.Register(
      "SelectionContainer", typeof(ISelectionContainer), typeof(SelectionContainerBehavior));

    public ISelectionContainer SelectionContainer
    {
      get { return (ISelectionContainer) GetValue(SelectionContainerProperty); }
      set { SetValue(SelectionContainerProperty, value); }
    }
    #endregion SelectionContainer
    #endregion Dependency Properties

    #region Overrides
    protected override void OnAttached()
    {
      base.OnAttached();

      var selector = AssociatedObject;
      selector.SelectionChanged += HandleSelectionChanged;
    }

    protected override void OnDetaching()
    {
      var selector = AssociatedObject;
      if (selector != null)
      {
        selector.SelectionChanged -= HandleSelectionChanged;
      }

      base.OnDetaching();
    }
    #endregion Overrides

    #region Event Handlers
    private void HandleSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var c = SelectionContainer;
      if (c == null)
      {
        Detach();
        return;
      }

      foreach (var removed in e.RemovedItems)
      {
        c.Unselect(removed);
      }

      foreach (var added in e.AddedItems)
      {
        c.Select(added);
      }
    }
    #endregion Event Handlers
  }
}