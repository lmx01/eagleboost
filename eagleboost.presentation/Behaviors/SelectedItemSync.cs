using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace eagleboost.presentation.Behaviors
{
  public class SelectedItemSync : Behavior<Border>
  {
    #region Dependency Properties

    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
      "Source", typeof(FrameworkElement), typeof(SelectedItemSync), new PropertyMetadata(OnSourceChanged));

    public FrameworkElement Source
    {
      get { return (FrameworkElement) GetValue(SourceProperty); }
      set { SetValue(SourceProperty, value); }
    }

    private static void OnSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((SelectedItemSync) obj).OnSourceChanged((FrameworkElement)e.NewValue);
    }

    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
      "SelectedItem", typeof(object), typeof(SelectedItemSync), new PropertyMetadata(OnSelectedItemChanged));

    public object SelectedItem
    {
      get { return GetValue(SelectedItemProperty); }
      set { SetValue(SelectedItemProperty, value); }
    }

    private static void OnSelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((SelectedItemSync)obj).OnSelectedItemChanged(e.NewValue);
    }
    #endregion

    protected override void OnAttached()
    {
      base.OnAttached();

      OnSelectedItemChanged(SelectedItem);
    }

    private void OnSourceChanged(FrameworkElement element)
    {
      element.MouseLeftButtonUp += HandleSourceMouseLeftButtonUp;
      element.MouseEnter += HandleSourceMouseEnter;
      element.MouseLeave += HandleSourceMouseLeave;
    }

    private void HandleSourceMouseLeave(object sender, MouseEventArgs e)
    {
      var border = AssociatedObject;
      if (SelectedItem != Source.DataContext)
      {
        border.BorderBrush = Brushes.Black;
        border.BorderThickness = new Thickness(1);
      }
    }

    private void HandleSourceMouseEnter(object sender, MouseEventArgs e)
    {
      var border = AssociatedObject;
      if (SelectedItem != Source.DataContext)
      {
        border.BorderBrush = Brushes.Orange;
        border.BorderThickness = new Thickness(2);
      }
    }

    private void HandleSourceMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      var dataContext = ((FrameworkElement) sender).DataContext;
      SelectedItem = dataContext;
    }

    private void OnSelectedItemChanged(object selectedItem)
    {
      var border = AssociatedObject;
      if (border != null && Source != null)
      {
        if (selectedItem == Source.DataContext)
        {
          border.BorderBrush = Brushes.Red;
          border.BorderThickness = new Thickness(2);
        }
        else
        {
          border.BorderBrush = Brushes.Black;
          border.BorderThickness = new Thickness(1);
        }
      }
    }
  }
}