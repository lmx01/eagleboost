// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 26 10:32 PM

using System;

namespace eagleboost.core.test.Threading
{
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.core.Threading.CancelationTokenTimeout;
  using Moq;
  using NUnit.Framework;
  using Prism.Logging;

  public class TaskExecutionLoggerTests
  {
    private const string TaskName = "Test";

    [Test]
    public async Task Task_01_Void_01_Timeout()
    {
      var loggerMock = new Mock<ILoggerFacade>();

      var tel = new TaskExecutionLogger();
      try
      {
        await tel.ExecuteAsync(TaskName, Helpers.ServerTimeoutEarlierAsync, loggerMock.Object, 1000);
      }
      catch
      {
        loggerMock.Verify(o => o.Log(TaskName + " is time out", Category.Exception, Priority.Medium));
      }
    }

    [Test]
    public async Task Task_01_Void_02_CallerTimeout()
    {
      var loggerMock = new Mock<ILoggerFacade>();

      var tel = new TaskExecutionLogger();
      try
      {
        await tel.ExecuteAsync(TaskName, Helpers.ServerTimeoutLaterAsync, loggerMock.Object, 1000);
      }
      catch
      {
        loggerMock.Verify(o => o.Log(TaskName + " is not returned in 1000ms", Category.Exception, Priority.Medium));
      }
    }

    [Test]
    public void Task_01_Void_03_CallerCancel()
    {
      var loggerMock = new Mock<ILoggerFacade>();

      var evt = new ManualResetEvent(false);

      var tel = new TaskExecutionLogger();
      CancellationTokenSource cts;
      tel.ExecuteAsync(TaskName, Helpers.ServerNeverReturnsAsync, loggerMock.Object, 3000, out cts).ContinueWith(t =>
      {
        loggerMock.Verify(o => o.Log(TaskName + " is canceled", Category.Exception, Priority.Medium));
        evt.Set();
      });

      Task.Delay(2000).ContinueWith(t => cts.Cancel());

      evt.WaitOne();
    }

    [Test]
    public async Task Task_01_Void_04_Success()
    {
      var loggerMock = new Mock<ILoggerFacade>();

      var tel = new TaskExecutionLogger();
      await tel.ExecuteAsync(TaskName, Helpers.ServerReturnsFastAsync, loggerMock.Object, 1000);
      loggerMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Task_02_Generic_01_Timeout()
    {
      var loggerMock = new Mock<ILoggerFacade>();

      var tel = new TaskExecutionLogger();
      try
      {
        await tel.ExecuteAsync(TaskName, Helpers.ServerTimeoutEarlierAsync_Generic, loggerMock.Object, 1000);
      }
      catch
      {
        loggerMock.Verify(o => o.Log(TaskName + " is time out", Category.Exception, Priority.Medium));
      }
    }

    [Test]
    public async Task Task_02_Generic_02_CallerTimeout()
    {
      var loggerMock = new Mock<ILoggerFacade>();

      var tel = new TaskExecutionLogger();
      try
      {
        await tel.ExecuteAsync(TaskName, Helpers.ServerTimeoutLaterAsync_Generic, loggerMock.Object, 1000);
      }
      catch
      {
        loggerMock.Verify(o => o.Log(TaskName + " is not returned in 1000ms", Category.Exception, Priority.Medium));
      }
    }

    [Test]
    public void Task_02_Generic_03_CallerCancel()
    {
      var loggerMock = new Mock<ILoggerFacade>();

      var evt = new ManualResetEvent(false);

      var tel = new TaskExecutionLogger();
      CancellationTokenSource cts;
      tel.ExecuteAsync(TaskName, Helpers.ServerNeverReturnsAsync_Generic, loggerMock.Object, 3000, out cts).ContinueWith(t =>
      {
        loggerMock.Verify(o => o.Log(TaskName + " is canceled", Category.Exception, Priority.Medium));
        evt.Set();
      });

      Task.Delay(2000).ContinueWith(t => cts.Cancel());

      evt.WaitOne();
    }

    [Test]
    public async Task Task_02_Generic_04_Success()
    {
      var loggerMock = new Mock<ILoggerFacade>();

      var tel = new TaskExecutionLogger();
      await tel.ExecuteAsync(TaskName, Helpers.ServerReturnsFastAsync_Generic, loggerMock.Object, 1000);
      loggerMock.VerifyNoOtherCalls();
    }
  }
}