// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 15 6:09 PM

namespace eagleboost.core.test.Threading
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using eagleboost.core.Extensions;
  using eagleboost.core.Threading;
  using eagleboost.core.Threading.CancelationTokenTimeout;
  using FluentAssertions;
  using NUnit.Framework;

  public class CancellableTaskHandlerTests
  {
    private async Task CalculateAsync(CancellationToken ct)
    {
      await Task.Delay(5000, ct);
    }

    [Test]
    public async Task Task_01_Execute_And_Cancel()
    {
      var h = new CancellableTaskHandler();
      var task = h.ExecuteAsync(CalculateAsync, 2000);
      h.Cancel();
      try
      {
        await task;
      }
      catch (Exception ex)
      {
        ex.Should().BeOfType<OperationCanceledByCallerException>();
      }
    }

    [Test]
    public async Task Task_02_Execute_And_Execute_And_Cancel()
    {
      var h = new CancellableTaskHandler();
      var task1 = h.ExecuteAsync(CalculateAsync, 2000);
      var task2 = h.ExecuteAsync(CalculateAsync, 2000);
      task1.Should().NotBe(task2);
      try
      {
        await task1;
      }
      catch (Exception ex)
      {
        ex.Should().BeOfType<OperationCanceledByCallerException>();
      }

      h.Cancel();
      try
      {
        await task2;
      }
      catch (Exception ex)
      {
        ex.Should().BeOfType<OperationCanceledByCallerException>();
      }
    }

    [Test]
    public async Task Task_03_Execute_Dispose()
    {
      var h = new CancellableTaskHandler();
      var task = h.ExecuteAsync(CalculateAsync, 2000);
      h.CastTo<IDisposable>().Dispose();
      try
      {
        await task;
      }
      catch (Exception ex)
      {
        ex.Should().BeOfType<OperationCanceledByCallerException>();
      }
    }
  }
}