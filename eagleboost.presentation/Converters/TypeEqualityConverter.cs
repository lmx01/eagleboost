namespace eagleboost.presentation.Converters
{
  using System;
  using System.Globalization;
  using System.Windows.Data;

  /// <summary>
  /// TypeEqualityConverter
  /// </summary>
  public class TypeEqualityConverter : IValueConverter
  {
    #region Public Methods
    public Type Type { get; set; }
    #endregion Public Methods
    
    #region ValueEqualityConverter
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
      {
        return false;
      }

      if (Type == null)
      {
        throw new InvalidOperationException("Type is not specified");
      }

      var match = Type.IsInstanceOfType(value);
      return match;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
    #endregion ValueEqualityConverter
  }
}