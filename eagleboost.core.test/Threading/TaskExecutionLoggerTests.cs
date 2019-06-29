// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 26 10:32 PM

using NSubstitute;

namespace eagleboost.core.test.Threading
{
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.core.Logging;
  using eagleboost.core.Threading.CancelationTokenTimeout;
  using NUnit.Framework;

  public class TaskExecutionLoggerTests
  {
    private const string TaskName = "Test";

    [Test]
    public async Task Task_01_Void_01_Timeout()
    {
      var logger = Substitute.For<ILoggerFacade>();

      var tel = new TaskExecutionLogger();
      try
      {
        await tel.ExecuteAsync(TaskName, Helpers.ServerTimeoutEarlierAsync, logger, 1000);
      }
      catch
      {
        logger.Received().Log(TaskName + " is time out", Category.Exception);
      }
    }

    [Test]
    public async Task Task_01_Void_02_CallerTimeout()
    {
      var logger = Substitute.For<ILoggerFacade>();

      var tel = new TaskExecutionLogger();
      try
      {
        await tel.ExecuteAsync(TaskName, Helpers.ServerTimeoutLaterAsync, logger, 1000);
      }
      catch
      {
        logger.Received().Log(TaskName + " is not returned in 1000ms", Category.Exception);
      }
    }

    [Test]
    public void Task_01_Void_03_CallerCancel()
    {
      var logger = Substitute.For<ILoggerFacade>();

      var evt = new ManualResetEvent(false);

      var tel = new TaskExecutionLogger();
      CancellationTokenSource cts;
      tel.ExecuteAsync(TaskName, Helpers.ServerNeverReturnsAsync, logger, 3000, out cts).ContinueWith(t =>
      {
        logger.Received().Log(TaskName + " is canceled", Category.Exception);
        evt.Set();
      });

      Task.Delay(2000).ContinueWith(t => cts.Cancel());

      evt.WaitOne();
    }

    [Test]
    public async Task Task_01_Void_04_Success()
    {
      var logger = Substitute.For<ILoggerFacade>();

      var tel = new TaskExecutionLogger();
      await tel.ExecuteAsync(TaskName, Helpers.ServerReturnsFastAsync, logger, 1000);
      logger.DidNotReceiveWithAnyArgs().Log("", Category.Exception);
    }

    [Test]
    public async Task Task_02_Generic_01_Timeout()
    {
      var logger = Substitute.For<ILoggerFacade>();

      var tel = new TaskExecutionLogger();
      try
      {
        await tel.ExecuteAsync(TaskName, Helpers.ServerTimeoutEarlierAsync_Generic, logger, 1000);
      }
      catch
      {
        logger.Received().Log(TaskName + " is time out", Category.Exception);
      }
    }

    [Test]
    public async Task Task_02_Generic_02_CallerTimeout()
    {
      var logger = Substitute.For<ILoggerFacade>();

      var tel = new TaskExecutionLogger();
      try
      {
        await tel.ExecuteAsync(TaskName, Helpers.ServerTimeoutLaterAsync_Generic, logger, 1000);
      }
      catch
      {
        logger.Received().Log(TaskName + " is not returned in 1000ms", Category.Exception);
      }
    }

    [Test]
    public void Task_02_Generic_03_CallerCancel()
    {
      var logger = Substitute.For<ILoggerFacade>();

      var evt = new ManualResetEvent(false);

      var tel = new TaskExecutionLogger();
      CancellationTokenSource cts;
      tel.ExecuteAsync(TaskName, Helpers.ServerNeverReturnsAsync_Generic, logger, 3000, out cts).ContinueWith(t =>
      {
        logger.Received().Log(TaskName + " is canceled", Category.Exception);
        evt.Set();
      });

      Task.Delay(2000).ContinueWith(t => cts.Cancel());

      evt.WaitOne();
    }

    [Test]
    public async Task Task_02_Generic_04_Success()
    {
      var logger = Substitute.For<ILoggerFacade>();

      var tel = new TaskExecutionLogger();
      await tel.ExecuteAsync(TaskName, Helpers.ServerReturnsFastAsync_Generic, logger, 1000);
      logger.DidNotReceiveWithAnyArgs().Log("", Category.Exception);
    }
  }
}