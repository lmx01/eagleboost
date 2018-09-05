// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-02 10:08 PM

namespace eagleboost.interaction.Common
{
  using System.Threading.Tasks;
  using System.Windows;
  using System.Windows.Threading;
  using eagleboost.interaction.Activities;
  using eagleboost.presentation.Controls;
  using eagleboost.presentation.Controls.Progress;
  using eagleboost.presentation.Extensions;
  using eagleboost.UserExperience.Threading;

  /// <summary>
  /// ProgressViewActivity
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class ProgressViewActivity<T> : DialogActivity<AsyncActivityResult<IProgressViewModel<T>>>
  {
    #region Declarations
    private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;
    #endregion Declarations

    #region Overrides
    protected override void ShowDialog()
    {
      DispatcherViewFactory.InvokeAsync("Secondary GUI", () =>
      {
        var viewModel = ActivityArgs.GetArgs<IProgressViewModel<T>>();
        viewModel.Completed += HandleCompleted;
        var window = new ViewControllerWindow
        {
          DataContext = viewModel,
          Content = new ProgressView {Width = 300, Margin = new Thickness(10, 5, 10, 10)},
          Title = viewModel.Header,
        }.RemoveIcon();

        var r = window.ShowDialog();
        var result = new AsyncActivityResult<IProgressViewModel<T>>(r.GetValueOrDefault(false), viewModel);
        Completion.TrySetResult(result);
      }).ConfigureAwait(false);
    }
    #endregion Overrides

    #region Event Handlers
    private void HandleCompleted(object sender, System.EventArgs e)
    {
      var vm = (IProgressViewModel<T>)sender;
      vm.Completed -= HandleCompleted;

      _dispatcher.CheckedInvoke(() =>
      {
        Task.Delay(500);
        vm.OkCommand.TryExecute(null);
      });
    }
    #endregion Event Handlers
  }
}