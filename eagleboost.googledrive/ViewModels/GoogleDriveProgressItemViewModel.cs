// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-05 11:33 PM

namespace eagleboost.googledrive.ViewModels
{
  using System.Threading;
  using eagleboost.core.Threading;
  using eagleboost.googledrive.Models;
  using eagleboost.presentation.Controls.Progress;

  public class GoogleDriveProgressItemViewModel : ProgressItemViewModel<GoogleDriveProgress>
  {
    #region ctors
    public GoogleDriveProgressItemViewModel(PauseTokenSource pts, CancellationTokenSource cts) : base(pts, cts)
    {
    }
    #endregion ctors

    #region Overrides
    protected override void DoReport(GoogleDriveProgress value)
    {
      Description = value.Status;
    }
    #endregion Overrides
  }
}