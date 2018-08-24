// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-24 9:32 AM

namespace eagleboost.presentation.Contracts
{
  using System;
  using System.Collections;
  using System.Collections.ObjectModel;

  /// <summary>
  /// ISelectedItemsSupport
  /// </summary>
  public interface ISelectedItemsSupport
  {
    IList SelectedItems { get; set; }

    event EventHandler SelectedItemsChanged;
  }

  /// <summary>
  /// ISelectedItemsSupport
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface ISelectedItemsSupport<T>
  {
    ObservableCollection<T> SelectedItems { get; }
  }
}