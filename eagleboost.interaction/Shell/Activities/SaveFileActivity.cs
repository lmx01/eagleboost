namespace eagleboost.interaction.Shell.Activities
{
  using eagleboost.interaction.Activities;
  using eagleboost.shell.Options;
  using Microsoft.Win32;

  public class SaveFileActivity : DialogActivity<string>
  {
    #region Overrides
    protected override void ShowDialog()
    {
      var saveFileDialog = new SaveFileDialog();
      var options = ActivityArgs.GetArgs<SaveFileOptions>();
      if (options != null)
      {
        saveFileDialog.AddExtension = options.AddExtension;
        saveFileDialog.InitialDirectory = options.InitialDirectory;
        saveFileDialog.DefaultExt = options.DefaultExt;
        saveFileDialog.Filter = options.Filter;
        saveFileDialog.FileName = options.FileName;
      }

      if (saveFileDialog.ShowDialog() == true)
      {
        Completion.TrySetResult(saveFileDialog.FileName);
      }
      else
      {
        Completion.TrySetResult(null);
      }
    }
    #endregion Overrides
  }
}