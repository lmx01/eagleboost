// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-26 9:10 PM

namespace eagleboost.presentation.Controls
{
  using System.Linq;
  using System.Windows;
  using System.Windows.Controls;
  using eagleboost.presentation.Extensions;

  /// <summary>
  /// NormalOrLastItemTemplateSelector
  /// </summary>
  public class NormalOrLastItemTemplateSelector : DataTemplateSelector
  {
    #region Public Properties
    public DataTemplate NormalItemTemplate { get; set; }

    public DataTemplate LastItemTemplate { get; set; }
    #endregion Public Properties

    #region Overrides
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      if (item == null || container == null)
      {
        return NormalItemTemplate;
      }

      var itemsControl = container.FindParent<ItemsControl>();
      var lastItem = itemsControl.ItemsSource.Cast<object>().Last();
      return item == lastItem ? LastItemTemplate : NormalItemTemplate;
    }
    #endregion Overrides
  }
}