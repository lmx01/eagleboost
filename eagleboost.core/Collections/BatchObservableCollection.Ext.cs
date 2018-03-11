namespace eagleboost.core.Collections
{
  using System.Collections.Generic;
  using eagleboost.core.Extensions;

  public static class BatchObservableCollectionExt
  {
    public static void Replace<T>(this BatchObservableCollection<T> col, IEnumerable<T> newItems)
    {
      using (col.SuspendNotification())
      {
        col.Clear();
        col.AddRange(newItems);
      }
    }
  }
}