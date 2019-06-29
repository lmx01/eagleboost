// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 25 7:11 PM

namespace eagleboost.core.Threading.CancelationTokenTimeout
{
  using System.Threading;
  using eagleboost.core.Logging;

  /// <summary>
  /// TaskExecutionLoggerBase
  /// </summary>
  public abstract class TaskExecutionLoggerBase
  {
    #region Declarations
    private CancellationTokenSource _cts;
    #endregion Declarations

    #region Private Methods
    private void DisposeCancellationToken()
    {
      if (_cts != null)
      {
        _cts.Cancel();
        _cts = null;
      }
    }
    #endregion Private Methods

    #region Protected Methods
    protected CancellationTokenSource ResetCancellationToken()
    {
      DisposeCancellationToken();

      return _cts = new CancellationTokenSource();
    }

    protected void LogTimeout(ILoggerFacade logger, TaskContext taskContext)
    {
      logger.Log(taskContext.TaskName + " is time out", Category.Exception);
    }

    protected void LogCallerTimeout(ILoggerFacade logger, TaskContext taskContext)
    {
      logger.Log(taskContext.TaskName + " is not returned in " + taskContext.MillisecondsTimeout + "ms", Category.Exception);
    }

    protected void LogCallerCancel(ILoggerFacade logger, TaskContext taskContext)
    {
      logger.Log(taskContext.TaskName + " is canceled", Category.Exception);
    }
    #endregion Protected Methods

    /// <summary>
    /// TaskContext
    /// </summary>
    protected class TaskContext
    {
      public TaskContext(string taskName, double millisecondsTimeout)
      {
        TaskName = taskName;
        MillisecondsTimeout = millisecondsTimeout;
      }

      public readonly string TaskName;

      public readonly double MillisecondsTimeout;
    }
  }
}