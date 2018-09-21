namespace eagleboost.googledrive.Views
{
  using System.Collections;
  using System.Windows;
  using eagleboost.presentation.Controls.TreeView;

  /// <summary>
  /// Interaction logic for GoogleDriveTreeView.xaml
  /// </summary>
  public partial class GoogleDriveTreeView
  {
    public GoogleDriveTreeView()
    {
      InitializeComponent();
      DriveTree.SelectedItemChanged += HandleSelectedItemChanged;
    }

    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
      "ItemsSource", typeof(IEnumerable), typeof(GoogleDriveTreeView), new PropertyMetadata(default(IEnumerable)));

    public IEnumerable ItemsSource
    {
      get { return (IEnumerable) GetValue(ItemsSourceProperty); }
      set { SetValue(ItemsSourceProperty, value); }
    }

    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
      "SelectedItem", typeof(ITreeNode), typeof(GoogleDriveTreeView));

    public ITreeNode SelectedItem
    {
      get { return (ITreeNode)GetValue(SelectedItemProperty); }
      set { SetValue(SelectedItemProperty, value); }
    }

    private void HandleSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      SelectedItem = (ITreeNode)e.NewValue;
    }
  }
}
