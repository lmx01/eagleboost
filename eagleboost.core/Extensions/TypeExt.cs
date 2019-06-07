// Author : Shuo Zhang
// 
// Creation :2018-03-09 16:53

namespace eagleboost.core.Extensions
{
  using System;
  using System.Linq;
  using System.Text;

  public static class TypeExt
  {
    public static bool IsSubclassOf<T>(this Type type)
    {
      return type.IsSubclassOf(typeof(T));
    }

    public static bool IsCompatiableWith<T>(this Type type)
    {
      return type == typeof(T) || typeof(T).IsAssignableFrom(type);
    }

    public static string GetName(this Type type)
    {
      if (type == null)
      {
        return null;
      }
      var fullName = type.FullName;
      return GetName(fullName);
    }

    internal static string GetName(string fullName)
    {
      if (fullName.IsNullOrEmpty())
      {
        return null;
      }

      var parts = fullName.Split('.');
      var last = parts[parts.Length - 1];
      var sb = new StringBuilder();
      for (var i = 0; i < parts.Length - 1; i++)
      {
        var part = parts[i];
        var uppers = part.Where(char.IsUpper).ToArray();
        if (uppers.Length > 0)
        {
          sb.Append(new string(uppers)).Append('.');
        }
        else
        {
          sb.Append(part[0]).Append('.');
        }
      }

      sb.Append(last);

      return sb.ToString();
    }
  }
}