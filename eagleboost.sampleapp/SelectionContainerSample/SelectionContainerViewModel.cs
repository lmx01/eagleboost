// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 12:17 PM

namespace eagleboost.sampleapp.SelectionContainerSample
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using eagleboost.core.Collections;
  using eagleboost.core.ComponentModel;

  public class SelectionContainerViewModel : NotifyPropertyChangedBase
  {
    public SelectionContainerViewModel()
    {
      var a = new DataItem("A");
      var b = new DataItem("B");
      var c = new DataItem("C");
      var d = new DataItem("D");
      var e = new DataItem("E");
      Items = new ObservableCollection<DataItem> {a, b, c, d, e};
      SingleSelectionContainer = new SingleSelectionContainer<DataItem>(b);
      RadioSelectionContainer = new RadioSelectionContainer<DataItem>(a);
      MultipleSelectionContainer = new MultipleSelectionContainer<DataItem>(new[] {c, d, e});
    }

    public ObservableCollection<DataItem> Items { get; set; }

    public SingleSelectionContainer<DataItem> SingleSelectionContainer { get; set; }

    public RadioSelectionContainer<DataItem> RadioSelectionContainer { get; set; }

    public MultipleSelectionContainer<DataItem> MultipleSelectionContainer { get; set; }
  }
}