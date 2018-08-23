// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-23 12:20 AM

namespace eagleboost.presentation.Extensions
{
  using System;
  using System.Threading.Tasks;
  using eagleboost.presentation.Contracts;

  public static class BusyStatusExt
  {
    public static async Task AutoReset(this IBusyStatus busyStatus, string status, Func<Task> taskFunc)
    {
      try
      {
        busyStatus.IsBusy = true;
        busyStatus.BusyStatus = status;
        await taskFunc().ConfigureAwait(false);
      }
      finally
      {
        busyStatus.IsBusy = false;
      }
    }

    public static async Task<T> AutoReset<T>(this IBusyStatus busyStatus, string status, Func<Task<T>> taskFunc)
    {
      try
      {
        busyStatus.IsBusy = true;
        busyStatus.BusyStatus = status;
        return await taskFunc().ConfigureAwait(false);
      }
      finally
      {
        busyStatus.IsBusy = false;
      }
    }
  }
}