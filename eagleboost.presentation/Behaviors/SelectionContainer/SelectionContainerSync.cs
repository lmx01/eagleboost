// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 12:03 PM

namespace eagleboost.presentation.Behaviors.SelectionContainer
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Controls.Primitives;
  using System.Windows.Interactivity;
  using eagleboost.core.Collections;

  /// <summary>
  /// SelectionContainerSync
  /// </summary>
  public class SelectionContainerSync<T> : Behavior<T> where T : Selector
  {
    #region Dependency Properties
    #region SelectionContainer
    public static readonly DependencyProperty SelectionContainerProperty = DependencyProperty.Register(
      "SelectionContainer", typeof(ISelectionContainer), typeof(SelectionContainerSync<T>), new PropertyMetadata(OnSelectionContainerChanged));

    public ISelectionContainer SelectionContainer
    {
      get { return (ISelectionContainer) GetValue(SelectionContainerProperty); }
      set { SetValue(SelectionContainerProperty, value); }
    }

    private static void OnSelectionContainerChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((SelectionContainerSync<T>)obj).OnSelectionContainerChanged();
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

    #region Virtuals
    protected virtual void OnSelectionContainerChanged()
    {
    }

    protected virtual void OnSelectorSelectionChanged(SelectionChangedEventArgs e)
    {
      var c = SelectionContainer;
      foreach (var removed in e.RemovedItems)
      {
        c.Unselect(removed);
      }

      foreach (var added in e.AddedItems)
      {
        c.Select(added);
      }
    }
    #endregion Virtuals

    #region Event Handlers
    private void HandleSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var c = SelectionContainer;
      if (c == null)
      {
        Detach();
        return;
      }

      OnSelectorSelectionChanged(e);
    }
    #endregion Event Handlers
  }

  /// <summary>
  /// SelectionContainerSync
  /// </summary>
  public class SelectionContainerSync : SelectionContainerSync<Selector>
  {
  }
}