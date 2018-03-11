using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace eagleboost.core.Extensions
{
    public static class EnumrableExt
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
        return source as IReadOnlyCollection<T> ?? new List<T>(source.Cast<T>());
      }
    }
}