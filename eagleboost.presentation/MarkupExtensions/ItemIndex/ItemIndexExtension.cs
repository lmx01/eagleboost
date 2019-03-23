// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 18 10:18 PM

using System;
using System.Windows;
using System.Windows.Markup;

namespace eagleboost.presentation.MarkupExtensions.ItemIndex
{
  /// <summary>
  /// ItemIndexExtension
  /// </summary>
  public partial class ItemIndexExtension : MarkupExtensionBase
  {
    private const string DefaultFormat = "F0";

    public ItemIndexExtension(Type itemsControlType)
    {
      ItemsControlType = itemsControlType;
    }

    #region Public Properties
    public Type ItemsControlType { get; set; }

    public DependencyProperty ItemsSourceProperty { get; set; }

    public string Format { get; set; }
    #endregion Public Properties

    #region Overrides
    public override object ProvideValue(IServiceProvider provider)
    {
      if (IsTemplated(provider))
      {
        return this;
      }

      FrameworkElement element;
      DependencyProperty property;
      if (TryGetTargetItems(provider, out element, out property))
      {
        new ItemTrack(element, property, ItemsControlType, ItemsSourceProperty, Format ?? DefaultFormat);
      }

      return null;
    }
    #endregion Overrides
  }
}