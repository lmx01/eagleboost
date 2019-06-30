// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 12:17 PM

namespace eagleboost.sampleapp.SelectionContainerSample
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Windows.Input;
  using eagleboost.core.Collections;
  using eagleboost.core.Commands;
  using eagleboost.core.ComponentModel;

  public class SelectionContainerViewModel : NotifyPropertyChangedBase
  {
    private DataItem a = new DataItem("A");
    private DataItem b = new DataItem("B");
    private DataItem c = new DataItem("C");
    private DataItem d = new DataItem("D");
    private DataItem e = new DataItem("E");

    public SelectionContainerViewModel()
    {
      Items = new ObservableCollection<DataItem> {a, b, c, d, e};
      SingleSelectionContainer = new SingleSelectionContainer<DataItem>(b);
      RadioSelectionContainer = new RadioSelectionContainer<DataItem>(a);
      MultipleSelectionContainer = new MultipleSelectionContainer<DataItem>(new[] {c, d, e});
      SelectCommand = new NotifiableCommand(HandleSelect, () => true);
      ClearCommand = new NotifiableCommand(HandleClear, () => true);
    }

    public ObservableCollection<DataItem> Items { get; private set; }

    public SingleSelectionContainer<DataItem> SingleSelectionContainer { get; private set; }

    public RadioSelectionContainer<DataItem> RadioSelectionContainer { get; private set; }

    public MultipleSelectionContainer<DataItem> MultipleSelectionContainer { get; private set; }

    public ICommand SelectCommand { get; private set; }

    public ICommand ClearCommand { get; private set; }

    private void HandleSelect()
    {
      MultipleSelectionContainer.Select(new[] { d, a, e });
    }

    private void HandleClear()
    {
      MultipleSelectionContainer.Clear();
    }
  }
}