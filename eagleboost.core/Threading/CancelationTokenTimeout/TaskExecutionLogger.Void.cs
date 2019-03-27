// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 27 12:01 AM

namespace eagleboost.core.Threading.CancelationTokenTimeout
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using Prism.Logging;

  /// <summary>
  /// TaskExecutionLogger
  /// </summary>
  public class TaskExecutionLogger : TaskExecutionLoggerBase
  {
    #region ITaskExecutionLogger
    public Task ExecuteAsync(string taskName, Func<CancellationToken, Task> taskFunc, ILoggerFacade logger, double millisecondsTimeout)
    {
      CancellationTokenSource cts;
      return ExecuteAsync(taskName, taskFunc, logger, millisecondsTimeout, out cts);
    }

    public Task ExecuteAsync(string taskName, Func<CancellationToken, Task> taskFunc, ILoggerFacade logger, double millisecondsTimeout, out CancellationTokenSource cts)
    {
      cts = ResetCancellationToken();
      return DoExecuteAsync(taskFunc, logger, new TaskContext(taskName, millisecondsTimeout), cts.Token);
    }
    #endregion ITaskExecutionLogger

    #region Private Methods
    private async Task DoExecuteAsync(Func<CancellationToken, Task> taskFunc, ILoggerFacade logger, TaskContext tc, CancellationToken ct)
    {
      try
      {
        await ct.TimeoutAsync(taskFunc, tc.MillisecondsTimeout);
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