using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using eagleboost.core.Collections;
using eagleboost.core.Extensions;

namespace eagleboost.presentation.Collections
{
  public class ItemPropertyChangedEventArgs : PropertyChangedEventArgs
  {
    public ItemPropertyChangedEventArgs(string propertyName) : base(propertyName)
    {
    }
  }

  public class FilterDescriptionCollection : BatchObservableCollection<IFilterDescription>
  {
    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnCollectionChanged(e);

      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          HookItemPropertyChanged(e.NewItems.AsReadOnlyCollection<IFilterDescription>());
          return;
        case NotifyCollectionChangedAction.Remove:
          UnhookItemPropertyChanged(e.OldItems.AsReadOnlyCollection<IFilterDescription>());
          return;
      }
    }

    #region Events
    public event EventHandler<ItemPropertyChangedEventArgs> ItemPropertyChanged;
    #endregion

    private void RaiseItemPropertyChanged(ItemPropertyChangedEventArgs e)
    {
      ItemPropertyChanged?.Invoke(this, e);
    }

    private void HookItemPropertyChanged(IReadOnlyCollection<IFilterDescription> items)
    {
      foreach (var item in items)
      {
        item.PropertyChanged -= HandleItemPropertyChanged;
        item.PropertyChanged += HandleItemPropertyChanged;
      }
    }

    private void UnhookItemPropertyChanged(IReadOnlyCollection<IFilterDescription> items)
    {
      foreach (var item in items)
      {
        item.PropertyChanged -= HandleItemPropertyChanged;
      }    
    }

    private void HandleItemPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      RaiseItemPropertyChanged(new ItemPropertyChangedEventArgs(e.PropertyName));
    }
  }
}