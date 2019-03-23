// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 18 10:22 PM


namespace eagleboost.presentation.MarkupExtensions.ItemIndex
{
  using System;
  using System.Collections;
  using System.Collections.Specialized;
  using System.Windows.Data;
  using System.Windows;
  using eagleboost.presentation.Extensions;

  /// <summary>
  /// ItemIndexExtension
  /// </summary>
  public partial class ItemIndexExtension
  {
    private class ItemTrack
    {
      private readonly FrameworkElement _element;
      private readonly DependencyProperty _property;
      private readonly Type _itemsControlType;
      private readonly DependencyProperty _itemsSourceProperty;
      private readonly string _format;
      private object _itemsSource;

      public ItemTrack(FrameworkElement element, DependencyProperty property, Type itemsControlType, DependencyProperty itemsSourceProperty, string format)
      {
        _element = element;
        _property = property;
        _itemsControlType = itemsControlType;
        _itemsSourceProperty = itemsSourceProperty;
        _format = format;

        _element.DataContextChanged += HandleDataContextChanged;
        if (_element.DataContext != null)
        {
          UpdateProperty(_element);
        }
      }

      #region Private Properties
      private object ItemsSource
      {
        get
        {
          if (_itemsSource == null)
          {
            var ancestor = (FrameworkElement)_element.FindParent(_itemsControlType);
            _itemsSource = ancestor.GetValue(_itemsSourceProperty);
            var collChanged = _itemsSource as INotifyCollectionChanged;
            if (collChanged != null)
            {
              collChanged.CollectionChanged += HandleCollectionChanged;
            }
          }

          return _itemsSource;
        }
      }

      private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
      {
        UpdateProperty(_element);
      }
      #endregion Private Properties

      #region Event Handlers
      private void HandleDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
      {
        var element = (FrameworkElement) sender;
        UpdateProperty(element);
      }
      #endregion Event Handlers

      #region Private Methods
      private void UpdateProperty(FrameworkElement element)
      {
        var dataContext = element.DataContext;
        if (dataContext != null)
        {
          var index = GetIndex(dataContext) + 1;
          element.SetValue(_property, index.ToString(_format));
        }
      }

      private int GetIndex(object item)
      {
        var view = ItemsSource as CollectionView;
        if (view != null)
        {
          return view.IndexOf(item);
        }

        var list = ItemsSource as IList;
        if (list != null)
        {
          return list.IndexOf(item);
        }

        throw new NotSupportedException();
      }
      #endregion Private Methods
    }
  }
}