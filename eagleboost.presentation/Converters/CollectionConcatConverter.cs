// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 12:26 PM

namespace eagleboost.presentation.Converters
{
  using System;
  using System.Collections;
  using System.Globalization;
  using System.Linq;
  using System.Windows.Data;

  public class CollectionConcatConverter : ConverterMarkupExtension<CollectionConcatConverter>, IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var c = value as IEnumerable;
      if (c != null)
      {
        return string.Join(",", c.Cast<object>());
      }

      return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}