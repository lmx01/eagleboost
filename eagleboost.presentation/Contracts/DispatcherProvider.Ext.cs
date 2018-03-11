namespace eagleboost.presentation.Contracts
{
  using System;
  using System.Windows.Threading;
  using eagleboost.presentation.Extensions;

  public static class DispatcherProviderExt
  {
    public static void BeginInvoke(this IDispatcherProvider p, Action action, DispatcherPriority priority = DispatcherPriority.Normal)
    {
      p.Dispatcher.BeginInvoke(action, priority);
    }

    public static void CheckedInvoke(this IDispatcherProvider p, Action action)
    {
      p.Dispatcher.CheckedInvoke(action);
    }
  }
}