namespace eagleboost.presentation.Converters
{
  using System;
  using System.Windows.Data;
  using System.Windows.Markup;

  public abstract class ConverterMarkupExtension<T> : MarkupExtension where T : ConverterMarkupExtension<T>, IValueConverter, new()
  {
    #region Statics
    private static readonly T Instance = new T();
    #endregion Statics

    #region Overrides
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return Instance;
    }
    #endregion Overrides
  }
}