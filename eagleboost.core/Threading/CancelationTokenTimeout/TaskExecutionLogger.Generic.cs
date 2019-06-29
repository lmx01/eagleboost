// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 26 11:59 PM

namespace eagleboost.core.Threading.CancelationTokenTimeout
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.core.Logging;

  /// <summary>
  /// ITaskExecutionLogger
  /// </summary>
  public interface ITaskExecutionLogger<T>
  {
    #region Methods
    Task<T> ExecuteAsync(string taskName, Func<CancellationToken, Task<T>> taskFunc, ILoggerFacade logger, double millisecondsTimeout);

    Task<T> ExecuteAsync(string taskName, Func<CancellationToken, Task<T>> taskFunc, ILoggerFacade logger, double millisecondsTimeout, out CancellationTokenSource cts);
    #endregion Methods
  }

  /// <summary>
  /// TaskExecutionLogger
  /// </summary>
  public class TaskExecutionLogger<T> : TaskExecutionLoggerBase, ITaskExecutionLogger<T>
  {
    #region ITaskExecutionLogger
    public Task<T> ExecuteAsync(string taskName, Func<CancellationToken, Task<T>> taskFunc, ILoggerFacade logger, double millisecondsTimeout)
    {
      CancellationTokenSource cts;
      return ExecuteAsync(taskName, taskFunc, logger, millisecondsTimeout, out cts);
    }

    public Task<T> ExecuteAsync(string taskName, Func<CancellationToken, Task<T>> taskFunc, ILoggerFacade logger, double millisecondsTimeout, out CancellationTokenSource cts)
    {
      cts = ResetCancellationToken();
      return DoExecuteAsync(taskFunc, logger, new TaskContext(taskName, millisecondsTimeout), cts.Token);
    }
    #endregion ITaskExecutionLogger

    #region Private Methods
    private async Task<T> DoExecuteAsync(Func<CancellationToken, Task<T>> taskFunc, ILoggerFacade logger, TaskContext tc, CancellationToken ct)
    {
      try
      {
        return await ct.TimeoutAsync(taskFunc, tc.MillisecondsTimeout);
      }
      catch (TimeoutException)
      {
        LogTimeout(logger, tc);
        throw;
      }
      catch (CallerOperationTimeoutException)
      {
        LogCallerTimeout(logger, tc);
        throw;
      }
      catch (OperationCanceledByCallerException)
      {
        LogCallerCancel(logger, tc);
        throw;
      }
    }
    #endregion Private Methods
  }
}