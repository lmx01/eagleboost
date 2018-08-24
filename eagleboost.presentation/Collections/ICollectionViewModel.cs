// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-24 10:03 AM

namespace eagleboost.presentation.Collections
{
  using System.Collections;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using eagleboost.presentation.Contracts;
  using eagleboost.presentation.Controls.DataGrids;

  /// <summary>
  /// ICollectionViewModel
  /// </summary>
  public interface ICollectionViewModel : ISelectedItemsSupport
  {
    IList Items { get; }

    ICollectionView ItemsView { get; }

    object SelectedItem { get; set; }
  }

  /// <summary>
  /// ICollectionViewModel
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface ICollectionViewModel<T> : ICollectionViewModel where T : class
  {
    new ObservableCollection<T> Items { get; }

    new T SelectedItem { get; set; }
  }
}