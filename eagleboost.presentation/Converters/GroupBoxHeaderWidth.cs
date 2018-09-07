// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-07 12:28 AM

namespace eagleboost.presentation.Converters
{
  using System;
  using System.Globalization;
  using System.Windows.Data;

  [ValueConversion(typeof(double), typeof(double))]
  public class GroupBoxHeaderWidth : StatefulConverterMarkupExtension<GroupBoxHeaderWidth>, IValueConverter
  {
    public double Margin { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value != null && (double)value > Margin)
        return (double)value - Margin;

      return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}