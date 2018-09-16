// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-02 10:08 PM

namespace eagleboost.interaction.Common
{
  using System;
  using System.ComponentModel;
  using System.Linq;
  using System.Threading.Tasks;
  using System.Windows;
  using eagleboost.core.Data;
  using eagleboost.interaction.Activities;
  using eagleboost.presentation.Controls;
  using eagleboost.presentation.Controls.Progress;
  using eagleboost.presentation.Extensions;
  using eagleboost.UserExperience.Threading;

  /// <summary>
  /// ProgressViewActivity
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class ProgressViewActivity<T> : DialogActivity<AsyncActivityResult<IProgressItemViewModel<T>>> where T : class
  {
    #region Declarations
    private static Window _window;
    #endregion Declarations

    #region Overrides
    protected override void ShowDialog()
    {
      var viewModel = ActivityArgs.GetArgs<IProgressItemViewModel<T>>();
      StartDialog(viewModel);
    }
    #endregion Overrides

    #region Private Methods
    private Window PrepareProgressView()
    {
      var options = ActivityArgs.GetArgs<ProgressViewOptions>();

      var window = new ViewControllerWindow
      {
        DataContext = new ProgressViewModel(),
        Content = new ProgressView {Width = 500, Margin = new Thickness(5)},
        Title = options != null ? options.Header : "Copy",
        ShowInTaskbar = true,
        Topmost = true,
      }.RemoveIcon().HideMinMaxButton();

      return window;
    }

    private void AddProgressItem(Window window, IProgressItemViewModel<T> itemViewModel)
    {
      var viewModel = (ProgressViewModel)window.DataContext;
      viewModel.ProgressItems.Add(itemViewModel);

      var cleanup = new DisposeManager();
      cleanup.AddEvent(h => itemViewModel.Completed += h, h => itemViewModel.Completed -= h,
        (EventHandler) ((s, e) => HandleCompletedOrCanceled(cleanup, window, viewModel, itemViewModel, true)));
      cleanup.AddEvent(h => itemViewModel.Canceled += h, h => itemViewModel.Canceled -= h,
        (EventHandler) ((s, e) => HandleCompletedOrCanceled(cleanup, window, viewModel, itemViewModel, false)));
    }

    private void HandleCompletedOrCanceled(DisposeManager cleanup, Window window, ProgressViewModel viewModel, IProgressItemViewModel<T> itemViewModel, bool isCompleted)
    {
      cleanup.Dispose();
      window.Dispatcher.CheckedInvoke(() =>
      {
        viewModel.ProgressItems.Remove(itemViewModel);
        if (viewModel.ProgressItems.Count == 0)
        {
          Task.Delay(1000);
          if (_window != null && _window.IsVisible)
          {
            viewModel.OkCommand.TryExecute(null);
          }
          _window = null;
          var result = new AsyncActivityResult<IProgressItemViewModel<T>>(isCompleted, itemViewModel);
          Completion.TrySetResult(result);
        }
      });
    }

    private void StartDialog(IProgressItemViewModel<T> itemViewModel)
    {
      DispatcherViewFactory.InvokeAsync("Secondary GUI", () =>
      {
        var window = _window;
        if (window == null)
        {
          window = _window = PrepareProgressView();
          AddProgressItem(window, itemViewModel);
          window.Closing += HandleWindowClosing;
          window.Show();
        }
        else
        {
          AddProgressItem(window, itemViewModel);
        }
      }).ConfigureAwait(false);
    }
    #endregion Private Methods

    #region Event Handlers
    private void HandleWindowClosing(object sender, CancelEventArgs e)
    {
      var window = (Window) sender;
      window.Closing -= HandleWindowClosing;

      _window = null;

      var vm = (ProgressViewModel) window.DataContext;
      foreach (var item in vm.ProgressItems.ToArray())
      {
        item.CancelCommand.TryExecute(null);
      }
    }
    #endregion Event Handlers
  }
}