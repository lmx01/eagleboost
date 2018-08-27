using System.ComponentModel;

namespace eagleboost.core.Extensions
{
  using System;
  using System.Linq.Expressions;

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

    public static bool Match<T>(this PropertyChangedEventArgs a, Expression<Func<T, object>> expr)
    {
      var member = expr.GetMember();
      return a.PropertyName == member;
    }
  }
}