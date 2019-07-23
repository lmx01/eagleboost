// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 22 9:50 PM

namespace eagleboost.googledrive.Views
{
  using System.ComponentModel;
  using System.Windows;
  using System.Windows.Data;
  using System.Windows.Threading;
  using eagleboost.googledrive.ViewModels;
  using eagleboost.presentation.Extensions;
  using eagleboost.UserExperience.Controls;

  /// <summary>
  /// GoogleDrivePopupTreeControl
  /// </summary>
  public class GoogleDrivePopupTreeControl : StaThreadContainer
  {
    #region Overrides
    protected override FrameworkElement CreateControl(object initParams)
    {
      var param = (GoogleDrivePopupTreeParams) initParams;
      var vm = new GoogleDriveTreeViewModel {UpdateFrequentFolder = true};
      vm.Initialize(param.DriveService);
      if (param.FrequentFiles != null)
      {
        vm.SetFrequentFiles(param.FrequentFiles);
      }
      vm.PropertyChanged += HandleViewModelPropertyChanged;
      var tree = new GoogleDriveTreeView {DataContext = vm};
      tree.SetBinding(GoogleDriveTreeView.ItemsSourceProperty, new Binding("ItemsView"));
      tree.SetBinding(GoogleDriveTreeView.SelectedItemProperty,
        new Binding("SelectedItem") {Mode = BindingMode.TwoWay});

      tree.Dispatcher.BeginInvoke(async () =>
      {
        await vm.InitializeAsync(false).ConfigureAwait(true);
        var path = param.Path;
        await vm.SelectAsync(path.Split(',')).ConfigureAwait(true);
      }, DispatcherPriority.Background);

      return tree;
    }

    #endregion Overrides

    #region Event Handlers
    private void HandleViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "SelectedItem")
      {
        var vm = (GoogleDriveTreeViewModel) sender;
        UpdateState(vm.SelectedItem);
      }
    }
    #endregion Event Handlers
  }
}