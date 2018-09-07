// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-06 11:42 PM

namespace eagleboost.presentation.Converters
{
  using System;
  using System.Globalization;
  using System.Windows.Data;

  /// <summary>
  /// BooleanInverse
  /// </summary>
  public class BooleanInverse : ConverterMarkupExtension<BooleanInverse>, IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var b = (bool) value;
      return !b;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}