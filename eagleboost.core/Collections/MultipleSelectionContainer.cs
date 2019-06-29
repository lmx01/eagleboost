// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 10:31 AM

namespace eagleboost.core.Collections
{
  using System.Collections.Generic;

  /// <summary>
  /// MultipleSelectionContainer
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public sealed class MultipleSelectionContainer<T> : SelectionContainerBase<T>
    where T : class
  {
    #region Declarations
    private HashSet<T> _selectedItems;
    #endregion Declarations

    #region ctors
    public MultipleSelectionContainer()
    {
    }

    public MultipleSelectionContainer(ICollection<T> selected) : base((selected))
    {
    }
    #endregion ctors

    #region Overrides
    protected override void ClearImpl()
    {
      _selectedItems.Clear();
    }

    protected override ICollection<T> CreateSelectedItems(ICollection<T> initialSelection)
    {
      return _selectedItems = new HashSet<T>(initialSelection);
    }

    protected override bool IsSelectedImpl(T item)
    {
      return _selectedItems.Contains(item);
    }

    protected override void SelectImpl(ICollection<T> items)
    {
      foreach (var item in items)
      {
        SelectImpl(item);
      }
    }

    protected override void SelectImpl(T toSelect)
    {
      if (CanSelectItem(toSelect))
      {
        _selectedItems.Add(toSelect);
      }
    }

    protected override void UnselectImpl(T toUnselect)
    {
      if (CanUnselectItem(toUnselect))
      {
        _selectedItems.Remove(toUnselect);
      }
    }

    protected override void UnselectImpl(ICollection<T> items)
    {
      foreach (var item in items)
      {
        UnselectImpl(item);
      }
    }

    protected override bool CanSelectItem(T item)
    {
      return !_selectedItems.Contains(item);
    }

    protected override bool CanUnselectItem(T item)
    {
      return _selectedItems.Contains(item);
    }
    #endregion Overrides
  }
}