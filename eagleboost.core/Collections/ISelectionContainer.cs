// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 10:13 AM

namespace eagleboost.core.Collections
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Windows.Input;

  /// <summary>
  /// SelectionContainerChangedEventArgs
  /// </summary>
  public class ItemsSelectedEventArgs : EventArgs
  {
    public ItemsSelectedEventArgs(ICollection items)
    {
      Items = items;
    }

    public readonly ICollection Items;
  }

  /// <summary>
  /// ItemsUnselectedEventArgs
  /// </summary>
  public class ItemsUnselectedEventArgs : EventArgs
  {
    public ItemsUnselectedEventArgs(ICollection items)
    {
      Items = items;
    }

    public readonly ICollection Items;
  }

  /// <summary>
  /// ISelectionContainer
  /// </summary>
  public interface ISelectionContainer
  {
    #region Properties
    IEnumerable SelectedItems { get; }

    bool this[object item] { get; }

    ICommand SelectCommand { get; }

    ICommand UnselectCommand { get; }
    #endregion Properties

    #region Methods
    void Clear();

    void Select(ICollection items);

    void Unselect(ICollection items);
    #endregion Methods

    #region Events
    event EventHandler ItemsCleared;

    event EventHandler<ItemsSelectedEventArgs> ItemsSelected;

    event EventHandler<ItemsUnselectedEventArgs> ItemsUnselected;
    #endregion Events
  }

  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface ISelectionContainer<T>
  {
    #region Properties
    IReadOnlyCollection<T> SelectedItems { get; }

    bool this[T item] { get; }
    #endregion Properties

    #region Methods
    void Clear();

    void Select(ICollection<T> items);

    void Unselect(ICollection<T> items);
    #endregion Methods
  }
}