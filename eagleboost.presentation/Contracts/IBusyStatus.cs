// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-22 11:52 PM

namespace eagleboost.presentation.Contracts
{
  public interface IBusyStatus
  {
    bool IsBusy { get; set; }

    string BusyStatus { get; set; }
  }
}