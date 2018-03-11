namespace eagleboost.presentation.Converters
{
  using System;
  using System.Globalization;
  using System.Windows.Data;

  /// <summary>
  /// ValueEqualityConverter
  /// </summary>
  public class ValueEqualityConverter : ConverterMarkupExtension<ValueEqualityConverter>, IValueConverter
  {
    #region ValueEqualityConverter
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return Equals(value, parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
    #endregion ValueEqualityConverter
  }
}