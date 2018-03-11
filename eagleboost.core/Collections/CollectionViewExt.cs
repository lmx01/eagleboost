namespace eagleboost.core.Collections
{
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;

  public static class EnumerableExt
  {
    public static T First<T>(this ICollectionView view)
    {
      return view.Cast<T>().First();
    }

    public static T Second<T>(this ICollectionView view)
    {
      return view.Cast<T>().Skip(1).First();
    }

    public static IReadOnlyCollection<T> GetView<T>(this ICollectionView view)
    {
      return view.Cast<T>().ToList();
    }
  }
}