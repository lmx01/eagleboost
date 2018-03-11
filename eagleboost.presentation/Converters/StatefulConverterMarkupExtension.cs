namespace eagleboost.presentation.Converters
{
  using System;
  using System.Windows.Data;
  using System.Windows.Markup;

  public class StatefulConverterMarkupExtension<T> : MarkupExtension where T : StatefulConverterMarkupExtension<T>, IValueConverter
  {
    #region Overrides
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return this;
    }
    #endregion Overrides
  }
}