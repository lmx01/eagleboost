// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-24 10:11 AM

namespace eagleboost.presentation.Collections
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Windows.Input;
  using eagleboost.core.Collections;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.Extensions;
  using eagleboost.presentation.Contracts;
  using Prism.Commands;

  /// <summary>
  /// CollectionViewModelBase
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class CollectionViewModelBase<T> : NotifyPropertyChangedBase, ICollectionViewModel<T>, ISelectedItemsSupport<T>, ISelectItemReceiver where T : class
  {
    #region Declarations
    private T _selectedItem;
    private ObservableCollection<T> _items;
    private ICollectionView _itemsView;
    private ICommand _itemSelectedCommand;
    #endregion Declarations

    #region ctors
    protected CollectionViewModelBase()
    {
      SelectionContainer = new MultipleSelectionContainer<T>();
      SelectionContainer.PropertyChanged += HandleSelectionContainerChanged;
    }
    #endregion ctors

    #region Components
    public MultipleSelectionContainer<T> SelectionContainer { get; private set; }
    #endregion Components

    #region ICollectionViewModel
    ISelectionContainer<T> ICollectionViewModel<T>.SelectionContainer
    {
      get { return SelectionContainer; }
    }
    #endregion ICollectionViewModel

    #region ISelectedItemsSupport
    IList ISelectedItemsSupport.SelectedItems
    {
      get { return (IList)SelectionContainer.SelectedItems; }
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

    public IReadOnlyCollection<T> SelectedItems
    {
      get { return SelectionContainer.SelectedItems; }
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

    public ObservableCollection<T> Items
    {
      get { return _items ?? (_items = GetItemsCollection()); }
    }

    public ICollectionView ItemsView
    {
      get { return _itemsView ?? (_itemsView = CreateItemsView()); }
    }
    #endregion ICollectionViewModel

    #region ISelectItemReceiver
    public ICommand SelectItemCommand
    {
      get { return _itemSelectedCommand ?? (_itemSelectedCommand = new DelegateCommand<T>(HandleItemSelected)); }
    }
    #endregion ISelectItemReceiver

    #region Event Handlers
    private void HandleSelectionContainerChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == SelectionContainer.Property(o=>o.SelectedItems))
      {
        RaiseSelectedItemsChanged();
      }
    }
    #endregion Event Handlers

    #region Virtuals
    protected virtual ObservableCollection<T> GetItemsCollection()
    {
      return new ObservableCollection<T>();
    }

    protected abstract ICollectionView CreateItemsView();

    protected virtual void OnItemSelected(T item)
    {
    }
    #endregion Virtuals

    #region Private Methods
    private void HandleItemSelected(T item)
    {
      OnItemSelected(item);
    }
    #endregion Private Methods
  }
}