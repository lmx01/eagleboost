namespace eagleboost.interaction.Shell.Activities
{
  using eagleboost.interaction.Activities;
  using Microsoft.Win32;

  public class OpenFileActivity : DialogActivity<string>
  {
    #region Overrides
    protected override void ShowDialog()
    {
      var openFileDialog = new OpenFileDialog();
      if (openFileDialog.ShowDialog() == true)
      {
        Completion.TrySetResult(openFileDialog.FileName);
      }
      else
      {
        Completion.TrySetResult(null);
      }
    }
    #endregion Overrides
  }
}