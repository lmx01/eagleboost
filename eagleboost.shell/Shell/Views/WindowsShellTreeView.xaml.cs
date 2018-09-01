namespace eagleboost.shell.Shell.Views
{
  using System.Windows;
  using System.Windows.Data;
  using eagleboost.presentation.Controls.TreeView;
  using eagleboost.presentation.Extensions;
  using eagleboost.shell.Shell.ViewModels;

  /// <summary>
  /// Interaction logic for WindowsShellTreeView.xaml
  /// </summary>
  public partial class WindowsShellTreeView
  {
    private readonly WindowsShellTreeViewModel _viewModel = new WindowsShellTreeViewModel();

    public WindowsShellTreeView()
    {
      InitializeComponent();

      ShellTree.SelectedItemChanged += HandleSelectedShellTreeItemChanged;
      this.SetupLoaded(() =>
      {
        if (BindingOperations.GetBinding(this, OptionsProperty) == null)
        {
          Options = new WindowsShellTreeViewOptions();
        }
      });
    }

    public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register(
      "Options", typeof(WindowsShellTreeViewOptions), typeof(WindowsShellTreeView),
      new PropertyMetadata(new WindowsShellTreeViewOptions(), OnOptionsChanged));

    public WindowsShellTreeViewOptions Options
    {
      get { return (WindowsShellTreeViewOptions) GetValue(OptionsProperty); }
      set { SetValue(OptionsProperty, value); }
    }

    private static void OnOptionsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((WindowsShellTreeView) obj).InitializeShellTree();
    }

    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
      "SelectedItem", typeof(ITreeNode), typeof(WindowsShellTreeView), new PropertyMetadata(OnSelectedItemChanged));

    public ITreeNode SelectedItem
    {
      get { return (ITreeNode) GetValue(SelectedItemProperty); }
      set { SetValue(SelectedItemProperty, value); }
    }

    private static void OnSelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
    }

    public static readonly DependencyProperty SelectedPathProperty = DependencyProperty.Register(
      "SelectedPath", typeof(string), typeof(WindowsShellTreeView), new PropertyMetadata(OnSelectedPathChanged));

    public string SelectedPath
    {
      get { return (string) GetValue(SelectedPathProperty); }
      set { SetValue(SelectedPathProperty, value); }
    }

    private static void OnSelectedPathChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((WindowsShellTreeView) obj).OnSelectedPathChanged();
    }

    private void InitializeShellTree()
    {
      _viewModel.InitializeAsync(false);
      ShellTree.ItemsSource = _viewModel.ItemsView;
    }

    private void HandleSelectedShellTreeItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      SelectedItem = (ITreeNode) e.NewValue;
    }

    private void OnSelectedPathChanged()
    {
      _viewModel.SelectAsync(SelectedPath.Split('\\'));
    }
  }
}
