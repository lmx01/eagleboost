// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 10:32 AM

namespace eagleboost.core.Collections
{
  using System;

  /// <summary>
  /// RadioSelectionContainer
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public sealed class RadioSelectionContainer<T> : SingleSelectionContainer<T>
    where T : class
  {
    #region ctors
    public RadioSelectionContainer()
    {
    }

    public RadioSelectionContainer(T selected) : base(selected)
    {
    }
    #endregion ctors

    #region Overrides
    protected override bool IsSelectedImpl(T item)
    {
      ////At least one item needs to be selected first, aka after this is initialized, caller should
      ////at least call Select once
      if (SelectedItems.Count == 0)
      {
        throw new InvalidOperationException("There's no items selected");
      }

      return base.IsSelectedImpl(item);
    }

    protected override bool CanUnselectItem(T toUnselect)
    {
      ////Unselected is not allowed, which means at least one item needs to be selected at any time
      return false;
    }

    protected override bool CanSelectItem(T item)
    {
      return !Equals(SelectedItem, item);
    }
    #endregion Overrides
  }
}