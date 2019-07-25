// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 22 9:50 PM

namespace eagleboost.googledrive.Views
{
  using System.ComponentModel;
  using System.Linq;
  using System.Windows;
  using System.Windows.Data;
  using System.Windows.Threading;
  using eagleboost.core.Extensions;
  using eagleboost.googledrive.ViewModels;
  using eagleboost.presentation.Extensions;
  using eagleboost.UserExperience.Controls;

  /// <summary>
  /// GoogleDrivePopupTreeControl
  /// </summary>
  public class GoogleDrivePopupTreeControl : StaThreadContainer<GoogleDrivePopupTreeParams, GoogleDrivePopupTreeState>
  {
    #region Declarations
    private Dispatcher _dispatcher;
    private GoogleDriveTreeView _treeView;
    private GoogleDriveTreeViewModel _viewModel;
    private bool _isSelectionChanged;
    #endregion Declarations

    #region Dependency Properties
    #region IsOpen
    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
      "IsOpen", typeof(bool), typeof(GoogleDrivePopupTreeControl), new PropertyMetadata(OnIsOpenChanged));

    public bool IsOpen
    {
      get { return (bool)GetValue(IsOpenProperty); }
      set { SetValue(IsOpenProperty, value); }
    }

    private static void OnIsOpenChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      var c = (GoogleDrivePopupTreeControl) obj;
      var isOpen = (bool) e.NewValue;
      c._dispatcher.BeginInvoke(() => c.OnIsOpenChanged(isOpen), DispatcherPriority.Background);
    }
    #endregion IsOpen
    #endregion Dependency Properties

    #region Overrides
    protected override FrameworkElement CreateControl(GoogleDrivePopupTreeParams initParams)
    {
      _dispatcher = Dispatcher.CurrentDispatcher;
      var param = initParams;
      var vm = _viewModel = new GoogleDriveTreeViewModel {UpdateFrequentFolder = true};
      vm.Initialize(param.DriveService);
      if (param.FrequentFiles != null)
      {
        vm.SetFrequentFiles(param.FrequentFiles);
      }
      vm.PropertyChanged += HandleViewModelPropertyChanged;
      var tree = _treeView = new GoogleDriveTreeView {DataContext = vm};
      tree.SetBinding(GoogleDriveTreeView.ItemsSourceProperty, new Binding(vm.Property(v => v.ItemsView)));
      tree.SetBinding(GoogleDriveTreeView.SelectedItemProperty, new Binding(vm.Property(v => v.SelectedItem)) {Mode = BindingMode.TwoWay});

      tree.Dispatcher.BeginInvoke(async () =>
      {
        await vm.InitializeAsync(false).ConfigureAwait(true);
        var path = param.Path;
        await vm.SelectAsync(path.Split(',')).ConfigureAwait(true);
        UpdateState();
      }, DispatcherPriority.Background);

      return tree;
    }
    #endregion Overrides

    #region Event Handlers
    private void HandleViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "SelectedItem")
      {
        _isSelectionChanged = true;
      }
    }

    private void OnIsOpenChanged(bool isOpen)
    {
      if (isOpen)
      {
        _treeView.Visibility = Visibility.Visible;
      }
      else
      {
        if (_isSelectionChanged)
        {
          _isSelectionChanged = false;
          UpdateState();
          _viewModel.RefreshCurrent();
        }
        _treeView.Visibility = Visibility.Hidden;
      }
    }
    #endregion Event Handlers

    #region Private Methods
    private void UpdateState()
    {
      var vm = _viewModel;
      var state = new GoogleDrivePopupTreeState
      {
        SelectedItem = vm.SelectedItem,
        FrequentFiles = vm.GetFrequentFiles().ToArray()
      };

      UpdateState(state);
    }
    #endregion Private Methods
  }
}