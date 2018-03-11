namespace eagleboost.presentation.Converters
{
  using System;
  using System.Globalization;
  using System.Windows.Data;

  public class HasTextConverter : ConverterMarkupExtension<HasTextConverter>, IValueConverter
  {
    #region IValueConverter
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var text = value as string;
      if (text == null)
      {
        return false;
      }

      if (text == string.Empty)
      {
        return false;
      }

      return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
    #endregion IValueConverter 
  }
}