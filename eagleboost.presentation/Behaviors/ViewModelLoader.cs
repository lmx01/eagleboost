namespace eagleboost.presentation.Behaviors
{
using System;
using System.Windows;
using System.Windows.Interactivity;
using eagleboost.core.Contracts;

  public class ViewModelLoader : Behavior<FrameworkElement>
  {
    public static readonly DependencyProperty ViewModelTypeProperty = DependencyProperty.Register(
      "ViewModelType", typeof(Type), typeof(ViewModelLoader));

    public Type ViewModelType
    {
      get { return (Type) GetValue(ViewModelTypeProperty); }
      set { SetValue(ViewModelTypeProperty, value); }
    }

    protected override void OnAttached()
    {
      base.OnAttached();

      AssociatedObject.Loaded += HandleLoaded;
    }

    private void HandleLoaded(object sender, RoutedEventArgs e)
    {
      var element = (FrameworkElement) sender;
      element.Loaded -= HandleLoaded;

      var viewModel = element.DataContext;
      if (viewModel == null && ViewModelType != null)
      {
        viewModel = Activator.CreateInstance(ViewModelType);
        element.DataContext = viewModel;
      }

      var startable = viewModel as IStartable;
      if (startable != null)
      {
        startable.Start();
      }
    }
  }
}