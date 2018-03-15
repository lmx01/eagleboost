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

      AssociatedObject.Loaded += HandleLoaded      ;
    }

    private void HandleLoaded(object sender, RoutedEventArgs e)
    {
      AssociatedObject.Loaded -= HandleLoaded;

      var viewModel = Activator.CreateInstance(ViewModelType);
      AssociatedObject.DataContext = viewModel;

      var startable = viewModel as IStartable;
      startable?.Start();
    }
  }
}