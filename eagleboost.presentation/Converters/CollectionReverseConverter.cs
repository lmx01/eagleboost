// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 25 10:58 PM

namespace eagleboost.presentation.Converters
{
  using System;
  using System.Collections;
  using System.Globalization;
  using System.Linq;
  using System.Windows.Data;

  /// <summary>
  /// CollectionReverseConverter
  /// </summary>
  public class CollectionReverseConverter : ConverterMarkupExtension<CollectionReverseConverter>, IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var c = value as IEnumerable;
      if (c != null)
      {
        return c.Cast<object>().Reverse();
      }

      return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}