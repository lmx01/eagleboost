// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 22 10:09 PM

namespace System.Threading
{
  using System;
  using System.Threading.Tasks;
  using eagleboost.core.Threading.CancelationTokenTimeout;

  public static class CanceledTaskExt
  {
    private static async Task HandleTask<TException>(this Task task, Action<TException> action) where TException : Exception
    {
      try
      {
        await task;
      }
      catch (Exception e)
      {
        if (e is TException)
        {
          action((TException)e);
        }
      }
    }

    public static Task OnTimeout(this Task task, Action<TimeoutException> action)
    {
      return task.HandleTask(action);
    }

    public static Task OnCallerTimeout(this Task task, Action<CallerOperationTimeoutException> action)
    {
      return task.HandleTask(action);
    }

    public static Task OnCallerCancel(this Task task, Action<OperationCanceledByCallerException> action)
    {
      return task.HandleTask(action);
    }
  }
}