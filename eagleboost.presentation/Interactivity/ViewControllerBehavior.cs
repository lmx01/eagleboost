using System.Windows;
using System.Windows.Interactivity;

namespace eagleboost.presentation.Interactivity
{
  using System.Windows.Interop;

  public class ViewControllerBehavior : Behavior<Window>
  {
    protected override void OnAttached()
    {
      base.OnAttached();

      var window = AssociatedObject;
      var dataContext = window.DataContext;
      if (dataContext != null)
      {
        SetupCallback(dataContext);
      }
      else
      {
        window.DataContextChanged += HandleDataContextChanged;
      }
    }

    private void HandleDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      ((Window)sender).DataContextChanged -= HandleDataContextChanged;

      if (e.NewValue != null)
      {
        SetupCallback(e.NewValue);
      }
    }

    private void SetupCallback(object dataContext)
    {
      var viewController = dataContext as IViewController;
      if (viewController != null)
      {
        viewController.AddCloseAction(CloseAction);
      }
    }

    private void CloseAction(bool? confirmed)
    {
      var window = AssociatedObject;
      if (window != null)
      {
        if (ComponentDispatcher.IsThreadModal)
        {
          window.DialogResult = confirmed;
        }
        window.Close();
      }
    }
  }
}