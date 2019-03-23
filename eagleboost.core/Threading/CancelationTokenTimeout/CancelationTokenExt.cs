// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 22 9:37 PM

namespace System.Threading
{
  using System;
  using System.Threading.Tasks;
  using eagleboost.core.Threading.CancelationTokenTimeout;

  public static class CancelationTokenExt
  {
    /// <summary>
    /// Throws OperationCanceledByCallerException when ct is canceled by caller, e.g. caller canceled the previous CancellationToken
    /// in order to initiate a new call
    /// Throws CallerOperationTimeoutException when the caller couldn't not get response withing timeout
    /// </summary>
    /// <param name="ct"></param>
    /// <param name="taskFunc"></param>
    /// <param name="millisecondsTimeout"></param>
    /// <returns></returns>
    public static Task Timeout(this CancellationToken ct, Func<CancellationToken, Task> taskFunc, double millisecondsTimeout)
    {
      return ct.Timeout(taskFunc, TimeSpan.FromMilliseconds(millisecondsTimeout));
    }

    /// <summary>
    /// Throws OperationCanceledByCallerException when ct is canceled by caller, e.g. caller canceled the previous CancellationToken
    /// in order to initiate a new call
    /// Throws CallerOperationTimeoutException when the caller couldn't not get response withing timeout
    /// </summary>
    /// <param name="ct"></param>
    /// <param name="taskFunc"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    public static async Task Timeout(this CancellationToken ct, Func<CancellationToken, Task> taskFunc, TimeSpan timeout)
    {
      using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct))
      {
        linkedCts.CancelAfter(timeout);

        try
        {
          var linkedCt = linkedCts.Token;
          await taskFunc(linkedCt).WithCancellation(linkedCt);
        }
        catch (OperationCanceledException e)
        {
          if (ct.IsCancellationRequested)
          {
            throw new OperationCanceledByCallerException(e);
          }

          throw new CallerOperationTimeoutException(e);
        }
      }
    }

    /// <summary>
    /// Throws OperationCanceledByCallerException when ct is canceled by caller, e.g. caller canceled the previous CancellationToken
    /// in order to initiate a new call
    /// Throws CallerOperationTimeoutException when the caller couldn't not get response withing timeout
    /// </summary>
    /// <param name="ct"></param>
    /// <param name="taskFunc"></param>
    /// <param name="millisecondsTimeout"></param>
    /// <returns></returns>
    public static Task<T> Timeout<T>(this CancellationToken ct, Func<CancellationToken, Task<T>> taskFunc, double millisecondsTimeout)
    {
      return ct.Timeout(taskFunc, TimeSpan.FromMilliseconds(millisecondsTimeout));
    }

    /// <summary>
    /// Throws OperationCanceledByCallerException when ct is canceled by caller, e.g. caller canceled the previous CancellationToken
    /// in order to initiate a new call
    /// Throws CallerOperationTimeoutException when the caller couldn't not get response withing timeout
    /// </summary>
    /// <param name="ct"></param>
    /// <param name="taskFunc"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    public static async Task<T> Timeout<T>(this CancellationToken ct, Func<CancellationToken, Task<T>> taskFunc, TimeSpan timeout)
    {
      using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct))
      {
        linkedCts.CancelAfter(timeout);

        try
        {
          var linkedCt = linkedCts.Token;
          return await taskFunc(linkedCt).WithCancellation(linkedCt);
        }
        catch (OperationCanceledException e)
        {
          if (ct.IsCancellationRequested)
          {
            throw new OperationCanceledByCallerException(e);
          }

          throw new CallerOperationTimeoutException(e);
        }
      }
    }

    /// <summary>
    /// In case the task never returns - https://devblogs.microsoft.com/pfxteam/how-do-i-cancel-non-cancelable-async-operations/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="task"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken ct)
    {
      var tcs = new TaskCompletionSource<bool>();
      using (ct.Register(s => ((TaskCompletionSource<bool>) s).TrySetResult(true), tcs))
      {
        if (task != await Task.WhenAny(task, tcs.Task))
        {
          throw new OperationCanceledException(ct);
        }
      }

      return await task;
    }

    /// <summary>
    /// In case the task never returns - https://devblogs.microsoft.com/pfxteam/how-do-i-cancel-non-cancelable-async-operations/
    /// </summary>
    /// <param name="task"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private static async Task WithCancellation(this Task task, CancellationToken ct)
    {
      var tcs = new TaskCompletionSource<bool>();
      using (ct.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
      {
        if (task != await Task.WhenAny(task, tcs.Task))
        {
          throw new OperationCanceledException(ct);
        }
      }

      await task;
    }
  }
}