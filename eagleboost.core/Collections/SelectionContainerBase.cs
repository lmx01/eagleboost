// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 10:33 AM

namespace eagleboost.core.Collections
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using System.Windows.Input;
  using eagleboost.core.Commands;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.Contracts;
  using eagleboost.core.Extensions;
  using eagleboost.core.Utils;

  /// <summary>
  /// SelectionContainerBase
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class SelectionContainerBase<T> : NotifyPropertyChangedBase, ISelectionContainer, ISelectionContainer<T>
    where T : class
  {
    #region Statics
    protected static readonly PropertyChangedEventArgs ItemArgs = new PropertyChangedEventArgs("Item[]");
    protected static readonly PropertyChangedEventArgs SelectedItemsArgs = GetChangedArgs<SelectionContainerBase<T>>(o => o.SelectedItems);
    #endregion Statics

    #region Declarations
    private IReadOnlyCollection<T> _selectedItems;
    private readonly IInvalidatableCommand _selectCommand;
    private readonly IInvalidatableCommand _unselectCommand;
    private readonly T[] _initialSelection;
    #endregion Declarations

    #region ctors
    protected SelectionContainerBase():this(new T[0])
    {
    }

    protected SelectionContainerBase(T selected) : this(new[] { selected })
    {
    }

    protected SelectionContainerBase(ICollection<T> selected)
    {
      _selectCommand = CreateSelectCommand();
      _unselectCommand = CreateUnselectCommand();
      _initialSelection = selected.ToArray();
    }
    #endregion ctors

    #region ISelectionContainer
    IEnumerable ISelectionContainer.SelectedItems
    {
      get { return SelectedItems; }
    }

    bool ISelectionContainer.this[object item]
    {
      get
      {
        var typed = ArgValidation.ThrowIfMismatch<T>(item, "item");
        return IsSelected(typed);
      }
    }

    public ICommand SelectCommand
    {
      get { return _selectCommand; }
    }

    public ICommand UnselectCommand
    {
      get { return _unselectCommand; }
    }

    void ISelectionContainer.Select(ICollection items)
    {
      Select(items.AsArray<T>());
    }

    void ISelectionContainer.Unselect(ICollection items)
    {
      Unselect(items.AsArray<T>());
    }

    public event EventHandler ItemsCleared;

    public event EventHandler<ItemsSelectedEventArgs> ItemsSelected;

    public event EventHandler<ItemsUnselectedEventArgs> ItemsUnselected;

    public IReadOnlyCollection<T> SelectedItems
    {
      get { return _selectedItems ?? (_selectedItems = CreateSelectedItems(_initialSelection)); }
    }

    public bool this[T item]
    {
      get { return IsSelected(item); }
    }

    public void Clear()
    {
      ClearImpl();
      RaiseItemsCleared();
    }

    public void Select(ICollection<T> items)
    {
      if (items == null || items.Count == 0)
      {
        return;
      }

      if (SelectedItems == null)
      {
        return;
      }

      if (items.Count == 1)
      {
        SelectImpl(items.First());
      }
      else
      {
        SelectImpl(items);
      }
      RaiseItemsSelected((ICollection)items);
    }

    public void Unselect(ICollection<T> items)
    {
      if (items == null || items.Count == 0)
      {
        return;
      }

      if (SelectedItems == null)
      {
        return;
      }

      if (items.Count == 1)
      {
        UnselectImpl(items.First());
      }
      else
      {
        UnselectImpl(items);
      }
      RaiseItemsUnselected((ICollection)items);
    }
    #endregion ISelectionContainer

    #region Protected Methods
    protected void EnsureSelectedItems()
    {
      if (SelectedItems != null)
      {
      }
    }

    protected bool IsSelected(T item)
    {
      if (item == null)
      {
        return false;
      }

      return IsSelectedImpl(item);
    }

    #endregion Protected Methods

    #region Virtuals
    protected abstract void ClearImpl();

    protected abstract IReadOnlyCollection<T> CreateSelectedItems(ICollection<T> initialSelection);

    protected abstract bool IsSelectedImpl(T item);

    protected abstract void SelectImpl(ICollection<T> toSelect);

    protected abstract void SelectImpl(T toSelect);

    protected abstract void UnselectImpl(T toUnselect);

    protected abstract void UnselectImpl(ICollection<T> toUnselect);

    protected abstract bool CanSelectItem(T item);

    protected abstract bool CanUnselectItem(T item);

    protected virtual void RaiseItemsCleared()
    {
      NotifySelectedItemsChanged();

      var handler = ItemsCleared;
      if (handler != null)
      {
        handler(this, EventArgs.Empty);
      }
    }

    protected virtual void RaiseItemsSelected(ICollection items)
    {
      NotifySelectedItemsChanged();

      var handler = ItemsSelected;
      if (handler != null)
      {
        handler(this, new ItemsSelectedEventArgs(items));
      }
    }

    protected virtual void RaiseItemsUnselected(ICollection items)
    {
      NotifySelectedItemsChanged();

      var handler = ItemsUnselected;
      if (handler != null)
      {
        handler(this, new ItemsUnselectedEventArgs(items));
      }
    }
    #endregion Virtuals

    #region Private Methods
    private NotifiableCommand<T> CreateSelectCommand()
    {
      return new NotifiableCommand<T>(HandleSelectItem, e => CanSelectItem(e));
    }

    private NotifiableCommand<T> CreateUnselectCommand()
    {
      return new NotifiableCommand<T>(HandleUnselectItem, e => CanUnselectItem(e));
    }

    private void HandleSelectItem(T item)
    {
      if (CanSelectItem(item))
      {
        SelectImpl(item);
        RaiseItemsSelected(new[] {item});
      }
    }

    private void HandleUnselectItem(T item)
    {
      if (CanUnselectItem(item))
      {
        UnselectImpl(item);
        RaiseItemsUnselected(new[] { item });
      }
    }

    private void NotifySelectedItemsChanged()
    {
      NotifyPropertyChanged(ItemArgs);
      NotifyPropertyChanged(SelectedItemsArgs);
      _selectCommand.Invalidate();
      _unselectCommand.Invalidate();
    }
    #endregion Private Methods
  }
}