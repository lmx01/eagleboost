namespace eagleboost.presentation.Extensions
{
  using System;
  using System.Threading.Tasks;
  using System.Windows.Threading;
  using eagleboost.core.Data;

  public static class DispatcherExt
  {
    public static void BeginInvoke(this Dispatcher d, Action action, DispatcherPriority priority = DispatcherPriority.Normal)
    {
      d.BeginInvoke(action, priority);
    }

    public static void CheckedInvoke(this Dispatcher d, Action action)
    {
      if (d.CheckAccess())
      {
        action();
      }
      else
      {
        d.BeginInvoke(action);
      }
    }

    public static Task ShutdownAsync(this Dispatcher d)
    {
      var tcs = new TaskCompletionSource<int>();

      var cleanup = new DisposeManager();

      EventHandler handler = (s, e) =>
      {
        cleanup.Dispose();
        tcs.TrySetResult(0);
      };

      cleanup.AddEvent(h => d.ShutdownFinished += h, h => d.ShutdownFinished -= h, handler);
      d.BeginInvokeShutdown(DispatcherPriority.Normal);

      return tcs.Task;
    }
  }
}