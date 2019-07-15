// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 14 8:27 PM

namespace eagleboost.presentation.Behaviors
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Interactivity;
  using eagleboost.core.Collections;

  /// <summary>
  /// TreeViewSelectedItems
  /// </summary>
  public class TreeViewSelectedItems : Behavior<TreeView>
  {
    #region Dependency Properties
    public static readonly DependencyProperty SelectionContainerProperty = DependencyProperty.Register(
      "SelectionContainer", typeof(ISelectionContainer), typeof(TreeViewSelectedItems));

    public ISelectionContainer SelectionContainer
    {
      get { return (ISelectionContainer)GetValue(SelectionContainerProperty); }
      set { SetValue(SelectionContainerProperty, value); }
    }
    #endregion Dependency Properties 

    #region Overrides
    protected override void OnAttached()
    {
      base.OnAttached();

      var tree = AssociatedObject;
      tree.SelectedItemChanged += HandleTreeSelectedItemChanged;
    }

    protected override void OnDetaching()
    {
      var tree = AssociatedObject;
      tree.SelectedItemChanged -= HandleTreeSelectedItemChanged;
      SelectionContainer = null;

      base.OnDetaching();
    }
    #endregion Overrides

    #region Event Handlers
    private void HandleTreeSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      var tree = (TreeView) sender;
      var container = SelectionContainer;
      if (container != null)
      {
        container.Select(tree.SelectedItem);
      }
    }
    #endregion Event Handlers
  }
}