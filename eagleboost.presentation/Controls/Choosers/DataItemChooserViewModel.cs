// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 02 7:15 PM

namespace eagleboost.presentation.Controls.Choosers
{
  using System.Collections;
  using System.Collections.ObjectModel;
  using eagleboost.presentation.Interactivity;

  public interface IDataItemChooserViewModel : IViewController
  {
    string Description { get; set; }

    object SelectedItem { get; set; }

    IEnumerable Items { get; }
  }

  /// <summary>
  /// DataItemChooserViewModel
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class DataItemChooserViewModel<T> : ViewController, IDataItemChooserViewModel
  {
    #region Declarations
    private T _selectedItem;
    private readonly ObservableCollection<T> _items = new ObservableCollection<T>();
    #endregion Declarations

    #region IDataItemChooserViewModel

    public string Description { get; set; }

    object IDataItemChooserViewModel.SelectedItem
    {
      get { return SelectedItem;}
      set { SelectedItem = (T)value; }
    }

    IEnumerable IDataItemChooserViewModel.Items
    {
      get { return Items; }
    }
    #endregion IDataItemChooserViewModel

    #region Public Properties
    public string Header { get; set; }

    public T SelectedItem
    {
      get { return _selectedItem; }
      set { SetValue(ref _selectedItem, value); }
    }

    public ObservableCollection<T> Items
    {
      get { return _items; }
    }
    #endregion Public Properties
  }
}