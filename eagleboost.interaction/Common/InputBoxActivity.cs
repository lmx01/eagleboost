// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-29 7:17 PM

namespace eagleboost.interaction.Common
{
  using System.Windows;
  using eagleboost.interaction.Activities;
  using eagleboost.presentation.Controls;
  using eagleboost.presentation.Controls.InputBoxes;
  using eagleboost.presentation.Extensions;

  /// <summary>
  /// InputBoxActivity
  /// </summary>
  public class InputBoxActivity : DialogActivity<AsyncActivityResult<InputBoxViewModel>>
  {
    #region Overrides
    protected override void ShowDialog()
    {
      var viewModel = ActivityArgs.GetArgs<InputBoxViewModel>() ?? new InputBoxViewModel();
      var window = new ViewControllerWindow
      {
        DataContext = viewModel,
        Content = new InputBox {Width = 300, Margin = new Thickness(10,5,10,10)},
        Title = viewModel.Header,
        SizeToContent = SizeToContent.WidthAndHeight,
        ResizeMode = ResizeMode.NoResize,
      }.RemoveIcon().HideMinMaxButton();
      var r = window.ShowDialog();
      var result = new AsyncActivityResult<InputBoxViewModel>(r.GetValueOrDefault(false), viewModel);
      Completion.TrySetResult(result);
    }
    #endregion Overrides
  }
}