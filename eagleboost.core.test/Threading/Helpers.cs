// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 26 10:55 PM

namespace eagleboost.core.test.Threading
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;

  public static class Helpers
  {
    public static Task ServerNeverReturnsAsync(CancellationToken ct)
    {
      var tcs = new TaskCompletionSource<int>();

      Task.Run(async () =>
      {
        while (true)
        {
          await Task.Delay(500);
        }
      });

      return tcs.Task;
    }

    public static Task<int> ServerNeverReturnsAsync_Generic(CancellationToken ct)
    {
      var tcs = new TaskCompletionSource<int>();

      Task.Run(async () =>
      {
        while (true)
        {
          await Task.Delay(500);
        }
      });

      return tcs.Task;
    }

    public static Task ServerTimeoutEarlierAsync(CancellationToken ct)
    {
      var tcs = new TaskCompletionSource<int>();

      Task.Run(async () =>
      {
        while (true)
        {
          await Task.Delay(500);
          tcs.TrySetException(new TimeoutException());
        }
      });

      return tcs.Task;
    }

    public static Task<int> ServerTimeoutEarlierAsync_Generic(CancellationToken ct)
    {
      var tcs = new TaskCompletionSource<int>();

      Task.Run(async () =>
      {
        while (true)
        {
          await Task.Delay(500);
          tcs.TrySetException(new TimeoutException());
        }
      });

      return tcs.Task;
    }

    public static Task ServerTimeoutLaterAsync(CancellationToken ct)
    {
      var tcs = new TaskCompletionSource<int>();

      Task.Run(async () =>
      {
        while (true)
        {
          await Task.Delay(3000);
          tcs.TrySetResult(0);
        }
      });

      return tcs.Task;
    }

    public static Task<int> ServerTimeoutLaterAsync_Generic(CancellationToken ct)
    {
      var tcs = new TaskCompletionSource<int>();

      Task.Run(async () =>
      {
        while (true)
        {
          await Task.Delay(3000);
          tcs.TrySetResult(0);
        }
      });

      return tcs.Task;
    }

    public static Task<bool> ServerReturnsFastAsync(CancellationToken ct)
    {
      return Task.Run(async () =>
      {
        await Task.Delay(50);
        return true;
      });
    }

    public static Task<int> ServerReturnsFastAsync_Generic(CancellationToken ct)
    {
      return Task.Run(async () =>
      {
        await Task.Delay(50);
        return 0;
      });
    }
  }
}