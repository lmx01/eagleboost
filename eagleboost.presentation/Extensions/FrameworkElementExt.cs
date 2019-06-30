namespace eagleboost.presentation.Extensions
{
  using System;
  using System.Diagnostics;
  using System.Reflection;
  using System.Windows;
  using System.Windows.Data;
  using eagleboost.core.Data;

  public static class FrameworkElementExt
  {
    private static readonly DependencyProperty DefaultStyleKeyProperty = (DependencyProperty)typeof(FrameworkElement)
      .GetField("DefaultStyleKeyProperty", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);

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

    public static void SetupLoaded(this FrameworkElement element, Action action)
    {
      if (element.IsLoaded)
      {
        action();
        return;
      }

      var cleanup = new DisposeManager();

      RoutedEventHandler handler = (s, e) =>
      {
        cleanup.Dispose();
        action();
      };

      cleanup.AddEvent(h => element.Loaded += h, h => element.Loaded -= h, handler);
    }

    public static void OverrideDefaultStyleKey()
    {
      var frame = new StackFrame(1);
      var method = frame.GetMethod();
      var type = method.DeclaringType;
      if (type == null)
      {
        throw new InvalidOperationException();
      }

      if (!type.IsSubclassOf(typeof(FrameworkElement)))
      {
        throw new InvalidOperationException();
      }

      DefaultStyleKeyProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(type));
    }

    public static void InitializeStyle(string styleFile = null)
    {
      var frame = new StackFrame(1);
      var method = frame.GetMethod();
      var type = method.DeclaringType;
      if (type == null)
      {
        throw new InvalidOperationException();
      }

      if (!type.IsSubclassOf(typeof(FrameworkElement)))
      {
        throw new InvalidOperationException();
      }

      var uri = styleFile;
      if (styleFile == null)
      {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyName = assembly.FullName.Split(',')[0];
        var typeName = type.FullName;
        typeName = typeName.Remove(0, assemblyName.Length + 1).Replace('.', '/');
        uri = "/" + assemblyName + ";component/" + typeName + ".xaml";
      }

      var rd = Application.LoadComponent(new Uri(uri, UriKind.RelativeOrAbsolute)) as ResourceDictionary;
      if (rd == null)
      {
        throw new InvalidOperationException("Cannot load resource dictionary from " + uri);
      }

      var style = rd[type] ?? rd[type.Name];
      FrameworkElement.StyleProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(style));
      DefaultStyleKeyProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(type));
    }
  }
}