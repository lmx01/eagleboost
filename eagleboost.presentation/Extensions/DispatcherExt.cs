namespace eagleboost.presentation.Extensions
{
  using System;
  using System.Windows.Threading;

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

  }
}