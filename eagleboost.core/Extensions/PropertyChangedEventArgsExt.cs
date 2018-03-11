using System.ComponentModel;

namespace eagleboost.core.Extensions
{
  public static class PropertyChangedEventArgsExt
  {
    public static bool Match(this PropertyChangedEventArgs a, PropertyChangedEventArgs b)
    {
      return a.PropertyName == b.PropertyName;
    }

    public static bool Match(this PropertyChangedEventArgs a, string propertyName)
    {
      return a.PropertyName == propertyName;
    }
  }
}