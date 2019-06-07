// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 02 7:14 PM

namespace eagleboost.interaction.Common
{
  using System.Windows;
  using eagleboost.interaction.Activities;
  using eagleboost.presentation.Controls;
  using eagleboost.presentation.Controls.Choosers;
  using eagleboost.presentation.Controls.InputBoxes;
  using eagleboost.presentation.Extensions;

  public class DataItemChooserActivity<T> : DialogActivity<AsyncActivityResult<DataItemChooserViewModel<T>>>
  {
    #region Overrides
    protected override void ShowDialog()
    {
      var viewModel = ActivityArgs.GetArgs<DataItemChooserViewModel<T>>();
      var window = new ViewControllerWindow
      {
        DataContext = viewModel,
        Content = new DataItemChooserView { Width = 300, Margin = new Thickness(10, 5, 10, 10) },
        Title = viewModel.Header,
      }.RemoveIcon().HideMinMaxButton();
      var r = window.ShowDialog();
      var result = new AsyncActivityResult<DataItemChooserViewModel<T>>(r.GetValueOrDefault(false), viewModel);
      Completion.TrySetResult(result);
    }
    #endregion Overrides
  }
}