// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : Aug 4 10:46 AM

namespace eagleboost.presentation.Behaviors
{
  using System.Linq;
  using System.Windows;
  using System.Windows.Interactivity;
  using System.Windows.Markup;
  using eagleboost.core.Collections;
  using eagleboost.core.ComponentModel;

  /// <summary>
  /// SelectedItemEffect
  /// </summary>
  [ContentProperty("Setters")]
  public class SelectedItemEffect : Behavior<FrameworkElement>
  {
    #region Declarations
    private SetterBaseCollection _setters = new SetterBaseCollection();
    #endregion Declarations
    
    #region Dependency Properties
    #region SelectionContainer
    public static readonly DependencyProperty SelectionContainerProperty = DependencyProperty.Register(
      "SelectionContainer", typeof(ISelectionContainer), typeof(SelectedItemEffect), new PropertyMetadata(OnSelectionContainerChanged));

    public ISelectionContainer SelectionContainer
    {
      get { return (ISelectionContainer)GetValue(SelectionContainerProperty); }
      set { SetValue(SelectionContainerProperty, value); }
    }

    private static void OnSelectionContainerChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((SelectedItemEffect)obj).OnSelectionContainerChanged();
    }
    #endregion SelectionContainer

    #region DataItem
    public static readonly DependencyProperty DataItemProperty = DependencyProperty.Register(
      "DataItem", typeof(object), typeof(SelectedItemEffect), new PropertyMetadata(OnDataItemChanged));

    public IItem DataItem
    {
      get { return (IItem)GetValue(DataItemProperty); }
      set { SetValue(DataItemProperty, value); }
    }

    private static void OnDataItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((SelectedItemEffect)obj).UpdateFrequentItem();
    }
    #endregion DataItem
    #endregion Dependency Properties

    #region Public Properties
    public SetterBaseCollection Setters
    {
      get { return _setters; }
    }
    #endregion Public Properties
    
    #region Event Handlers
    private void OnSelectionContainerChanged()
    {
      var c = SelectionContainer;
      if (c != null)
      {
        c.ItemsSelected += HandleItemsSelected;
      }
      UpdateFrequentItem();
    }

    private void HandleItemsSelected(object sender, ItemsSelectedEventArgs e)
    {
      var item = e.Items.OfType<IItem>().FirstOrDefault();
      if (item != null)
      {
        UpdateFrequentItem();
      }
    }

    private void UpdateFrequentItem()
    {
      var dataItem = DataItem;
      var c = SelectionContainer;
      var element = AssociatedObject;
      if (dataItem == null || c == null || element == null)
      {
        return;
      }

      if (c[dataItem.Id])
      {
        foreach (var setter in _setters.Cast<Setter>())
        {
          element.SetValue(setter.Property, setter.Value);
        }
      }
    }
    #endregion Event Handlers
  }
}