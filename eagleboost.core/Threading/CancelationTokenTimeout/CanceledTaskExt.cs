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

    private static bool CheckException<TException>(this Task task) where TException : Exception
    {
      var ex = task.Exception;
      if (ex != null && ex.InnerException is TException)
      {
        return true;
      }

      return false;
    }

    public static bool IsTimeout(this Task task)
    {
      return task.CheckException<TimeoutException>();
    }

    public static bool IsCallerTimeout(this Task task)
    {
      return task.CheckException<CallerOperationTimeoutException>();
    }

    public static bool IsCallerCancel(this Task task)
    {
      return task.CheckException<OperationCanceledByCallerException>();
    }
  }
}