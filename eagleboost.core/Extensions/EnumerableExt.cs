namespace eagleboost.core.Extensions
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  public static class EnumerableExt
  {
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
      foreach (var item in items)
      {
        collection.Add(item);
      }
    }

    public static IReadOnlyCollection<T> AsReadOnlyCollection<T>(this IEnumerable source)
    {
      if (source == null)
      {
        return null;
      }

      return source as IReadOnlyCollection<T> ?? new List<T>(source.Cast<T>());
    }

    public static T[] AsArray<T>(this IEnumerable source)
    {
      if (source == null)
      {
        return null;
      }

      return source as T[] ?? source.Cast<T>().ToArray();
    }

    public static string ConcatAll<T>(this IEnumerable<T> source, string separator)
    {
      if (source == null)
      {
        return null;
      }

      return string.Join(separator, source);
    }
  }
}