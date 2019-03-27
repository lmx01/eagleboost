// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 22 9:52 PM

namespace eagleboost.core.test.Threading
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.core.Threading.CancelationTokenTimeout;
  using FluentAssertions;
  using NUnit.Framework;

  public class CancelationTokenTimeoutTests
  {
    [Test]
    public async Task Task_01_Server_01_Never_Returns_Timeout()
    {
      var cts = new CancellationTokenSource();
      try
      {
        await cts.Token.TimeoutAsync(Helpers.ServerNeverReturnsAsync, TimeSpan.FromMilliseconds(2000));
      }
      catch (Exception e)
      {
        e.Should().BeOfType<CallerOperationTimeoutException>();
      }
    }

    [Test]
    public async Task Task_01_Server_02_Timeout_Earlier()
    {
      var cts = new CancellationTokenSource();
      try
      {
        await cts.Token.TimeoutAsync(Helpers.ServerTimeoutEarlierAsync, TimeSpan.FromMilliseconds(2000));
      }
      catch (Exception e)
      {
        e.Should().BeOfType<TimeoutException>();
      }
    }


    [Test]
    public async Task Task_02_Caller_01_Timeout()
    {
      var cts = new CancellationTokenSource();
      try
      {
        await cts.Token.TimeoutAsync(Helpers.ServerTimeoutLaterAsync, TimeSpan.FromMilliseconds(2000));
      }
      catch (Exception e)
      {
        e.Should().BeOfType<CallerOperationTimeoutException>();
      }
    }

    [Test]
    public async Task Task_02_Caller_02_Cancel()
    {
      var cts = new CancellationTokenSource();
      try
      {
        var task = cts.Token.TimeoutAsync(Helpers.ServerNeverReturnsAsync, TimeSpan.FromMilliseconds(2000));
        cts.Cancel();
        await task;
      }
      catch (Exception e)
      {
        e.Should().BeOfType<OperationCanceledByCallerException>();
      }
    }

    [Test]
    [TestCase(2000d)]
    public async Task Task_03_OnTimeout_01_Server_Timeout(double timeout)
    {
      var cts = new CancellationTokenSource();
      var isTimeout = false;
      var t = TimeSpan.FromMilliseconds(timeout);

      await cts.Token.TimeoutAsync(Helpers.ServerNeverReturnsAsync, t)
        .OnTimeout(e => isTimeout = true);
      isTimeout.Should().BeFalse();

      await cts.Token.TimeoutAsync(Helpers.ServerTimeoutLaterAsync, t)
        .OnTimeout(e => isTimeout = true);
      isTimeout.Should().BeFalse();

      await cts.Token.TimeoutAsync(Helpers.ServerTimeoutEarlierAsync, t)
        .OnTimeout(e => isTimeout = true);
      isTimeout.Should().BeTrue();
    }

    [Test]
    [TestCase(2000d)]
    public async Task Task_03_OnCallerTimeout_01_Server_Never_Returns(double timeout)
    {
      var cts = new CancellationTokenSource();
      var isTimeout = false;
      var t = TimeSpan.FromMilliseconds(timeout);

      await cts.Token.TimeoutAsync(Helpers.ServerTimeoutEarlierAsync, t)
        .OnCallerTimeout(e => isTimeout = true);
      isTimeout.Should().BeFalse();

      await cts.Token.TimeoutAsync(Helpers.ServerNeverReturnsAsync, t)
        .OnCallerTimeout(e => isTimeout = true);
      isTimeout.Should().BeTrue();
    }

    [Test]
    [TestCase(2000d)]
    public async Task Task_03_OnCallerTimeout_02_Server_Timeout_Later(double timeout)
    {
      var cts = new CancellationTokenSource();
      var isTimeout = false;
      var t = TimeSpan.FromMilliseconds(timeout);

      await cts.Token.TimeoutAsync(Helpers.ServerTimeoutEarlierAsync, t)
        .OnCallerTimeout(e => isTimeout = false);
      isTimeout.Should().BeFalse();

      await cts.Token.TimeoutAsync(Helpers.ServerTimeoutLaterAsync, t)
        .OnCallerTimeout(e => isTimeout = true);
      isTimeout.Should().BeTrue();
    }

    [Test]
    [TestCase(1000d)]
    [TestCase(2000d)]
    public async Task Task_03_OnCallerCancel_01_Server_Never_Returns(double timeout)
    {
      var cts = new CancellationTokenSource();
      var isCalledCanceled = false;
      var t = TimeSpan.FromMilliseconds(timeout);

      var task = cts.Token.TimeoutAsync(Helpers.ServerNeverReturnsAsync, t);
      cts.Cancel();
      await task.OnCallerCancel(e => isCalledCanceled = true);

      isCalledCanceled.Should().BeTrue();
    }

    [Test]
    [TestCase(1000d)]
    [TestCase(2000d)]
    public async Task Task_03_OnCallerCancel_02_Server_Timeout_Earlier(double timeout)
    {
      var cts = new CancellationTokenSource();
      var isCalledCanceled = false;
      var t = TimeSpan.FromMilliseconds(timeout);

      var task = cts.Token.TimeoutAsync(Helpers.ServerTimeoutEarlierAsync, t);
      cts.Cancel();
      await task.OnCallerCancel(e => isCalledCanceled = true);
      isCalledCanceled.Should().BeTrue();
    }

    [Test]
    [TestCase(1000d)]
    [TestCase(2000d)]
    public async Task Task_03_OnCallerCancel_03_Server_Timeout_Later(double timeout)
    {
      var cts = new CancellationTokenSource();
      var isCalledCanceled = false;
      var t = TimeSpan.FromMilliseconds(timeout);

      var task = cts.Token.TimeoutAsync(Helpers.ServerTimeoutLaterAsync, t);
      cts.Cancel();
      await task.OnCallerCancel(e => isCalledCanceled = true);

      isCalledCanceled.Should().BeTrue();
    }

    [Test]
    [TestCase(1000d)]
    [TestCase(2000d)]
    public async Task Task_04_Sucess_01_No_OnTimeout(double timeout)
    {
      var cts = new CancellationTokenSource();
      var isTimeout = false;
      var t = TimeSpan.FromMilliseconds(timeout);

      Task<bool> task;
      task = cts.Token.TimeoutAsync(Helpers.ServerReturnsFastAsync, t);
      var timeoutTask = task.OnTimeout(e => isTimeout = true);
      await timeoutTask;
      isTimeout.Should().BeFalse();

      var result = await task;
      result.Should().BeTrue();
    }

    [Test]
    [TestCase(1000d)]
    [TestCase(2000d)]
    public async Task Task_04_Sucess_02_No_OnCallerTimeout(double timeout)
    {
      var cts = new CancellationTokenSource();
      var isCallerTimeout = false;
      var t = TimeSpan.FromMilliseconds(timeout);

      Task<bool> task;
      task = cts.Token.TimeoutAsync(Helpers.ServerReturnsFastAsync, t);
      var callerTimeoutTask = task.OnCallerTimeout(e => isCallerTimeout = true);

      await callerTimeoutTask;
      isCallerTimeout.Should().BeFalse();

      var result = await task;
      result.Should().BeTrue();
    }

    [Test]
    [TestCase(1000d)]
    [TestCase(2000d)]
    public async Task Task_04_Sucess_03_No_OnCallerCancel(double timeout)
    {
      var cts = new CancellationTokenSource();
      var isCallerCanceled = false;
      var t = TimeSpan.FromMilliseconds(timeout);

      Task<bool> task;
      task = cts.Token.TimeoutAsync(Helpers.ServerReturnsFastAsync, t);
      var callerCancelTask = task.OnCallerCancel(e => isCallerCanceled = true);

      await callerCancelTask;
      isCallerCanceled.Should().BeFalse();

      var result = await task;
      result.Should().BeTrue();
    }
  }
}
