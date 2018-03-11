namespace eagleboost.shell.ShellTreeView.Views
{
  using System;
  using System.Windows;
  using eagleboost.shell.ShellTreeView.Contracts;
  using eagleboost.shell.ShellTreeView.ViewModels;

  /// <summary>
  /// Interaction logic for ShellTreeView.xaml
  /// </summary>
  public partial class ShellTreeView
  {
    private readonly ShellTreeViewModel _viewModel = new ShellTreeViewModel();

    public ShellTreeView()
    {
      InitializeComponent();

      ShellTree.SelectedItemChanged += HandleSelectedShellTreeItemChanged;
    }

    public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register(
      "Options", typeof(ShellTreeViewOptions), typeof(ShellTreeView),
      new PropertyMetadata(new ShellTreeViewOptions(), OnOptionsChanged));

    public ShellTreeViewOptions Options
    {
      get { return (ShellTreeViewOptions) GetValue(OptionsProperty); }
      set { SetValue(OptionsProperty, value); }
    }

    private static void OnOptionsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((ShellTreeView) obj).InitializeShellTree();
    }

    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
      "SelectedItem", typeof(IShellTreeItem), typeof(ShellTreeView), new PropertyMetadata(OnSelectedItemChanged));

    public IShellTreeItem SelectedItem
    {
      get { return (IShellTreeItem) GetValue(SelectedItemProperty); }
      set { SetValue(SelectedItemProperty, value); }
    }

    private static void OnSelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
    }

    public static readonly DependencyProperty SelectedPathProperty = DependencyProperty.Register(
      "SelectedPath", typeof(string), typeof(ShellTreeView), new PropertyMetadata(OnSelectedPathChanged));

    public string SelectedPath
    {
      get { return (string) GetValue(SelectedPathProperty); }
      set { SetValue(SelectedPathProperty, value); }
    }

    private static void OnSelectedPathChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((ShellTreeView) obj).OnSelectedPathChanged();
    }

    private void InitializeShellTree()
    {
      _viewModel.Initialize(Options);
      ShellTree.ItemsSource = _viewModel.ShellItemsView;
    }

    private void HandleSelectedShellTreeItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      SelectedItem = (IShellTreeItem) e.NewValue;
    }

    private void OnSelectedPathChanged()
    {
      _viewModel.SetPath(SelectedPath);
    }
  }
}
