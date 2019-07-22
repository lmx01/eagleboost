using System;

namespace eagleboost.core.Extensions
{
  public static class StringExt
  {
    public static bool IsNullOrEmpty(this string str)
    {
      return string.IsNullOrEmpty(str);
    }

    public static bool HasValue(this string str)
    {
      return !str.IsNullOrEmpty();
    }

    public static bool ContainsNoCase(this string str, string pattern)
    {
      return str.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0;
    }
    
    public static bool EqualsNoCase(this string str, string pattern)
    {
      return str.Equals(pattern, StringComparison.OrdinalIgnoreCase);
    }

    public static bool EndsWithNoCase(this string str, string pattern)
    {
      return str.EndsWith(pattern, StringComparison.OrdinalIgnoreCase);
    }

    public static bool StartsWithNoCase(this string str, string pattern)
    {
      return str.StartsWith(pattern, StringComparison.OrdinalIgnoreCase);
    }
    
    public static string TryAddPostFix(this string str, string postFix)
    {
      if (!str.EndsWith(postFix))
      {
        return str + postFix;
      }

      return str;
    }

    public static int CompareNoCase(this string l, string r)
    {
      return string.Compare(l, r, StringComparison.InvariantCultureIgnoreCase);
    }
  }
}