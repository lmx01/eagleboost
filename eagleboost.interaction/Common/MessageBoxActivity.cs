// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 31 9:49 PM

namespace eagleboost.interaction.Common
{
  using System.Windows;
  using eagleboost.interaction.Activities;
  using eagleboost.presentation.Controls.MessageBoxes;

  /// <summary>
  /// MessageBoxActivity
  /// </summary>
  public class MessageBoxActivity : DialogActivity<AsyncActivityResult<MessageBoxViewModel>>
  {
    #region Overrides
    protected override void ShowDialog()
    {
      var viewModel = ActivityArgs.GetArgs<MessageBoxViewModel>() ?? new MessageBoxViewModel();
      viewModel.Result = MessageBox.Show(viewModel.Text, viewModel.Header, viewModel.MessageBoxButton, viewModel.MessageBoxImage);
      var result = new AsyncActivityResult<MessageBoxViewModel>(true, viewModel);
      Completion.TrySetResult(result);
    }
    #endregion Overrides
  }
}