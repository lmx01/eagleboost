namespace eagleboost.presentation.Controls.Indicators
{
  using System;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Data;
  using eagleboost.core.Extensions;
  using eagleboost.presentation.Extensions;

  public class BusyIndicator : UserControl
  {
    #region Declarations
    private IDisposable _adorner;
    private readonly BusyIndicatorOverlay _overlay;
    #endregion Declarations

    #region ctors
    public BusyIndicator()
    {
      _overlay = new BusyIndicatorOverlay();
      _overlay.SetBinding(BusyIndicatorOverlay.IsBusyProperty, new Binding(IsBusyProperty.Name) {Source = this});
      _overlay.SetBinding(BusyIndicatorOverlay.BusyContentProperty, new Binding(BusyContentProperty.Name) { Source = this });
      _overlay.SetBinding(BusyIndicatorOverlay.BusyContentTemplateProperty, new Binding(BusyContentTemplateProperty.Name) { Source = this });
      _overlay.SetBinding(BusyIndicatorOverlay.BusyContentTemplateSelectorProperty, new Binding(BusyContentTemplateSelectorProperty.Name) { Source = this });
      _overlay.SetBinding(BusyIndicatorOverlay.OverlaySourceProperty, new Binding(ContentProperty.Name) {Source = this});

      this.SetupDataContextChanged<BusyStatusReceiver>(OnDataContextChanged);
    }
    #endregion ctors

    public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
      "IsBusy", typeof(bool), typeof(BusyIndicator), new PropertyMetadata(OnIsBusyChanged));

    public bool IsBusy
    {
      get { return (bool) GetValue(IsBusyProperty); }
      set { SetValue(IsBusyProperty, value); }
    }

    private static void OnIsBusyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((BusyIndicator)obj).OnIsBusyChanged();
    }

    public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register(
      "BusyContent", typeof(object), typeof(BusyIndicator));

    public object BusyContent
    {
      get { return GetValue(BusyContentProperty); }
      set { SetValue(BusyContentProperty, value); }
    }

    public static readonly DependencyProperty BusyContentTemplateProperty = DependencyProperty.Register(
      "BusyContentTemplate", typeof(DataTemplate), typeof(BusyIndicator));

    public DataTemplate BusyContentTemplate
    {
      get { return (DataTemplate)GetValue(BusyContentTemplateProperty); }
      set { SetValue(BusyContentTemplateProperty, value); }
    }

    public static readonly DependencyProperty BusyContentTemplateSelectorProperty = DependencyProperty.Register(
      "BusyContentTemplateSelector", typeof(DataTemplateSelector), typeof(BusyIndicator));

    public DataTemplateSelector BusyContentTemplateSelector
    {
      get { return (DataTemplateSelector)GetValue(BusyContentTemplateSelectorProperty); }
      set { SetValue(BusyContentTemplateSelectorProperty, value); }
    }
    #region Private Methods
    private void OnIsBusyChanged()
    {
      if (_adorner != null)
      {
        _adorner.Dispose();
      }

      if (IsBusy)
      {
        _adorner = OverlayAdorner.Overlay(this, _overlay);
      }
    }

    private void OnDataContextChanged(BusyStatusReceiver context)
    {
      if (context != null)
      {
        if (BindingOperations.GetBinding(this, IsBusyProperty) == null &&
            BindingOperations.GetBinding(this, BusyContentProperty) == null)
        {
          SetBinding(IsBusyProperty, new Binding(context.Property(o => o.IsBusy)) {Source = context});
          SetBinding(BusyContentProperty, new Binding(context.Property(o => o.BusyStatus)) { Source = context });
        }
      }
    }
    #endregion Private Methods
  }
}
