namespace eagleboost.presentation.Extensions
{
  using System;
  using System.Windows;
  using System.Windows.Data;
  using eagleboost.core.Data;

  public static class FrameworkElementExt
  {
    public static void SetBinding(this FrameworkElement element, DependencyProperty dp, DependencyProperty sourceDp, DependencyObject source, BindingMode mode = BindingMode.TwoWay)
    {
      element.SetBinding(dp, new Binding(sourceDp.Name) {Source = source, Mode = mode});
    }

    public static void SetupDataContextChanged(this FrameworkElement element, Action<object> action)
    {
      element.SetupDataContextChanged<object>(action);
    }

    public static void SetupDataContextChanged<T>(this FrameworkElement element, Action<T> action) where T : class
    {
      var context = element.DataContext;
      if (context != null)
      {
        if (action != null)
        {
          action.Invoke(context as T);
        }
        return;
      }

      var cleanup = new DisposeManager();

      DependencyPropertyChangedEventHandler handler = (s, e) =>
      {
        cleanup.Dispose();
        if (action != null)
        {
          action.Invoke(e.NewValue as T);
        }
      };

      cleanup.AddEvent(h => element.DataContextChanged += h, h => element.DataContextChanged -= h, handler);
    }
  }
}