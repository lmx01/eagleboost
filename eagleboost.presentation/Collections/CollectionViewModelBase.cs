﻿// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-24 10:11 AM

namespace eagleboost.presentation.Collections
{
  using System;
  using System.Collections;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Linq;
  using eagleboost.core.ComponentModel;
  using eagleboost.presentation.Contracts;
  using eagleboost.presentation.Controls.DataGrids;

  /// <summary>
  /// CollectionViewModelBase
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class CollectionViewModelBase<T> : NotifyPropertyChangedBase, ICollectionViewModel<T>, ISelectedItemsSupport<T> where T : class
  {
    #region Statics
    protected static readonly PropertyChangedEventArgs SelectedItemsArgs = GetChangedArgs<CollectionViewModelBase<T>>(o => o.SelectedItems);
    #endregion Statics

    #region Declarations
    private T _selectedItem;
    private ObservableCollection<T> _selectedItems;
    private ObservableCollection<T> _items;
    private ICollectionView _itemsView;
    #endregion Declarations

    #region ISelectedItemsSupport
    IList ISelectedItemsSupport.SelectedItems
    {
      get { return SelectedItems; }
      set
      {
        _selectedItems = new ObservableCollection<T>(value.Cast<T>());
        NotifyPropertyChanged(SelectedItemsArgs.PropertyName);
      }
    }

    public event EventHandler SelectedItemsChanged;

    protected virtual void RaiseSelectedItemsChanged()
    {
      var handler = SelectedItemsChanged;
      if (handler != null)
      {
        handler(this, EventArgs.Empty);
      }
    }

    public ObservableCollection<T> SelectedItems
    {
      get { return _selectedItems; }
      set
      {
        if (SetValue(ref _selectedItems, value))
        {
          RaiseSelectedItemsChanged();
        }
      }
    }
    #endregion ISelectedItemsSupport

    #region ICollectionViewModel
    IList ICollectionViewModel.Items
    {
      get { return Items; }
    }

    object ICollectionViewModel.SelectedItem
    {
      get { return SelectedItem; }
      set { SelectedItem = (T)value; }
    }

    public T SelectedItem
    {
      get { return _selectedItem; }
      set { SetValue(ref _selectedItem, value); }
    }

    public virtual ObservableCollection<T> Items
    {
      get { return _items ?? (_items = GetItemsCollection()); }
    }

    public ICollectionView ItemsView
    {
      get { return _itemsView ?? (_itemsView = CreateItemsView()); }
    }
    #endregion ICollectionViewModel

    #region Virtuals

    protected virtual ObservableCollection<T> GetItemsCollection()
    {
      return new ObservableCollection<T>();
    }

    protected abstract ICollectionView CreateItemsView();
    #endregion Virtuals
  }
}