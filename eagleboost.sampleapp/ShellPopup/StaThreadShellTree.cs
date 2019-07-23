// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 22 8:51 PM

namespace eagleboost.sampleapp.ShellPopup
{
  using System.ComponentModel;
  using System.Windows;
  using eagleboost.presentation.Extensions;
  using eagleboost.shell.Shell.ViewModels;
  using eagleboost.shell.Shell.Views;
  using eagleboost.UserExperience.Controls;

  /// <summary>
  /// StaThreadShellTree
  /// </summary>
  public class StaThreadShellTree : StaThreadContainer
  {
    #region Overrides
    protected override FrameworkElement CreateControl(object initParams)
    {
      var tree = new WindowsShellTreeView();
      tree.SetupDataContextChanged(e => OnDataContextChanged(tree, e));
      return tree;
    }
    #endregion Overrides

    #region Event Handlers
    private void OnDataContextChanged(WindowsShellTreeView tree, object dataContext)
    {
      var vm = (WindowsShellTreeViewModel) dataContext;
      if (vm != null)
      {
        vm.PropertyChanged += HandleViewModelPropertyChanged;
      }
    }

    private void HandleViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "SelectedItem")
      {
        var vm = (WindowsShellTreeViewModel)sender;
        UpdateState(vm.SelectedItem);
      }
    }
    #endregion Event Handlers
  }
}