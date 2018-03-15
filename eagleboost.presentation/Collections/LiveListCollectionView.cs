namespace eagleboost.presentation.Collections
{
  using System;
  using System.Collections;
  using System.Collections.Specialized;
  using System.ComponentModel;
  using System.Windows.Data;

  public class LiveListCollectionView : ListCollectionView
  {
    private FilterDescriptionCollection _filterDescriptionCollection;
    
    public LiveListCollectionView(IList list) : base(list)
    {
      FilterDescriptions = new FilterDescriptionCollection();
      Filter = PassesFilterDescriptions;
    }

    public FilterDescriptionCollection FilterDescriptions
    {
      get { return _filterDescriptionCollection; }
      set
      {
        if (value == null)
        {
          throw new ArgumentNullException();
        }
        
        if (_filterDescriptionCollection != null)
        {
          _filterDescriptionCollection.CollectionChanged -= HandleFilterDescriptionsChanged;
          _filterDescriptionCollection.ItemPropertyChanged -= HandleFilterDescriptionChanged;
        }

        _filterDescriptionCollection = value;
        _filterDescriptionCollection.CollectionChanged += HandleFilterDescriptionsChanged;
        _filterDescriptionCollection.ItemPropertyChanged += HandleFilterDescriptionChanged;
      }
    }

    private void HandleFilterDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      RefreshOnFilterChanged();
    }

    private void HandleFilterDescriptionChanged(object sender, PropertyChangedEventArgs e)
    {
      RefreshOnFilterChanged();
    }

    private void RefreshOnFilterChanged()
    {
      if (IsAddingNew || IsEditingItem)
      {
        throw new InvalidOperationException();
      }

      RefreshOrDefer();
    }

    public override bool PassesFilter(object item)
    {
      return base.PassesFilter(item) && PassesFilterDescriptions(item);
    }

    private bool PassesFilterDescriptions(object item)
    {
      var d = FilterDescriptions;
      if (d != null)
      {
        foreach (var description in d)
        {
          if (!description.Match(item))
          {
            return false;
          }
        }
      }

      return true;
    }
  }
}
