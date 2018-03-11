// Author : Shuo Zhang
// 
// Creation :2018-03-09 16:53

namespace eagleboost.core.Extensions
{
  using System;

  public static class TypeExt
  {
    public static bool IsSubclassOf<T>(this Type type)
    {
      return type.IsSubclassOf(typeof(T));
    }

    public static bool IsCompatiableWith<T>(this Type type)
    {
      return type == typeof(T) || type.IsSubclassOf(typeof(T));
    }
  }
}