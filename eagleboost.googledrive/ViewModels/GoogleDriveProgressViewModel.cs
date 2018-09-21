// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-04 11:51 PM

namespace eagleboost.googledrive.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using System.Threading;
  using eagleboost.core.Extensions;
  using eagleboost.core.Threading;
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Models;
  using eagleboost.presentation.Controls.Progress;

  /// <summary>
  /// GoogleDriveProgressViewModel
  /// </summary>
  public class GoogleDriveProgressViewModel : ProgressItemViewModel<GoogleDriveProgress>
  {
    #region ctors
    public GoogleDriveProgressViewModel(PauseTokenSource pts, CancellationTokenSource cts) : base(pts, cts)
    {
      Items = new List<GoogleDriveProgressItemViewModel>();
    }
    #endregion ctors

    #region Public Properties
    public List<GoogleDriveProgressItemViewModel> Items { get; private set; }

    public IDictionary<IGoogleDriveFile, int> TotalFiles { get; set; }
    #endregion Public Properties

    #region Overrides
    protected override void OnInitialize()
    {
      base.OnInitialize();

      foreach (var i in Items)
      {
        i.PropertyChanged += HandleItemPropertyChanged;
      }
    }

    protected override void DoReport(GoogleDriveProgress value)
    {
      Console.WriteLine(value.Status);
      Description = value.Status;
      Current = value.Count;
      if (TotalFiles != null)
      {
        int index;
        if (TotalFiles.TryGetValue(value.Current, out index))
        {
          Current = index + 1;
          Total = TotalFiles.Count;
          TimeRemaining = TimeSpan.FromSeconds(Total.Value - Current * TimeElapsed.Value.TotalSeconds / Current);
          Progress = (double)Current / TotalFiles.Count;
          NotifyPropertyChanged(HasRemainingArgs);
          NotifyPropertyChanged(AverageSpeedArgs);
        }
      }
      else
      {
        Progress = 0;
      }
    }
    #endregion Overrides

    #region Event Handlers
    private void HandleItemPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      var item = (GoogleDriveProgressItemViewModel) sender;
      if (e.Match<GoogleDriveProgressItemViewModel>(i => i.State))
      {
        DoReport(item.State);
      }
    }
    #endregion Event Handlers
  }
}