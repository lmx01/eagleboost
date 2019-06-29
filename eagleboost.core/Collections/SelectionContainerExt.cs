// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 11:02 AM

namespace eagleboost.core.Collections
{
  public static class SelectionContainerExt
  {
    public static void Select(this ISelectionContainer container, object item)
    {
      container.Select(new[] { item });
    }

    public static void Unselect(this ISelectionContainer container, object item)
    {
      container.Unselect(new[] { item });
    }

    public static void Select<T>(this ISelectionContainer<T> container, T item)
    {
      container.Select(new [] {item});
    }

    public static void Unselect<T>(this ISelectionContainer<T> container, T item)
    {
      container.Unselect(new[] { item });
    }
  }
}