namespace eagleboost.core.Extensions
{
  using System;
  using System.Linq.Expressions;

  public static class ObjectExt
  {
    public static string NullableToString(this object obj)
    {
      return obj != null ? obj.ToString() : null;
    }

    public static string Property<T>(this T obj, Expression<Func<T, object>> expr)
    {
      return expr.GetMember();
    }

    public static T CastTo<T>(this object obj) where T : class
    {
      return (T) obj;
    }
  }
}