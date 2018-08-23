// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-22 11:51 PM

namespace eagleboost.presentation.Controls.Indicators
{
  using System;
  using eagleboost.core.ComponentModel;
  using eagleboost.presentation.Contracts;

  public class BusyStatusReceiver : NotifyPropertyChangedBase, IBusyStatus, IProgress<string>
  {
    #region Declarations
    public bool _isBusy;
    public string _busyStatus;
    #endregion Declarations

    #region IBusyStatus
    public bool IsBusy
    {
      get { return _isBusy; }
      set { SetValue(ref _isBusy, value); }
    }

    public string BusyStatus
    {
      get { return _busyStatus; }
      set { SetValue(ref _busyStatus, value); }
    }
    #endregion IBusyStatus

    #region IProgress
    public void Report(string value)
    {
      BusyStatus = value;
    }
    #endregion IProgress
  }
}