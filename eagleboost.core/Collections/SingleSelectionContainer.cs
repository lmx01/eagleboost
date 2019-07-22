// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 10:17 AM

namespace eagleboost.core.Collections
{
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// SingleSelectionContainer
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class SingleSelectionContainer<T> : SelectionContainerBase<T>
    where T : class
  {
    #region Declarations
    private List<T> _selectedItems;
    private T _selectedItem;
    #endregion Declarations

    #region ctors
    public SingleSelectionContainer()
    {
      EnsureSelectedItems();
    }

    public SingleSelectionContainer(T selected) : base(selected)
    {
      EnsureSelectedItems();
    }
    #endregion ctors

    #region Public Properties
    public T SelectedItem
    {
      get { return _selectedItem; }
    }
    #endregion Public Properties

    #region Overrides
    protected sealed override void ClearImpl()
    {
      _selectedItems.Clear();
      _selectedItem = null;
    }

    protected sealed override IReadOnlyCollection<T> CreateSelectedItems(ICollection<T> initialSelection)
    {
      return _selectedItems = new List<T>(initialSelection);
    }

    protected override bool IsSelectedImpl(T item)
    {
      return Equals(_selectedItem, item);
    }

    protected sealed override void SelectImpl(ICollection<T> items)
    {
      var toSelect = items.First();
      SelectImpl(toSelect);
    }

    protected sealed override void UnselectImpl(ICollection<T> items)
    {
      var toUnselect = items.First();
      UnselectImpl(toUnselect);
    }

    protected override bool CanSelectItem(T item)
    {
      return !Equals(_selectedItem, item);
    }

    protected override bool CanUnselectItem(T item)
    {
      return Equals(_selectedItem, item);
    }

    protected override void SelectImpl(T toSelect)
    {
      _selectedItem = toSelect;
      if (_selectedItems.Count == 0)
      {
        _selectedItems.Add(toSelect);
      }
      else
      {
        _selectedItems[0] = toSelect;
      }
    }

    protected override void UnselectImpl(T toUnselect)
    {
      if (CanUnselectItem(toUnselect))
      {
        _selectedItems.Clear();
        _selectedItem = null;
      }
    }
    #endregion Overrides
  }
}