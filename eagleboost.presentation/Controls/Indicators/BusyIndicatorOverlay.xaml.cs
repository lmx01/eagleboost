namespace eagleboost.presentation.Controls.Indicators
{
  using System.Windows;

  /// <summary>
  /// Interaction logic for BusyIndicatorOverlay.xaml
  /// </summary>
  public partial class BusyIndicatorOverlay
  {
    public BusyIndicatorOverlay()
    {
      InitializeComponent();
    }

    public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
      "IsBusy", typeof(bool), typeof(BusyIndicatorOverlay));

    public bool IsBusy
    {
      get { return (bool)GetValue(IsBusyProperty); }
      set { SetValue(IsBusyProperty, value); }
    }

    public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register(
      "BusyContent", typeof(object), typeof(BusyIndicatorOverlay));

    public object BusyContent
    {
      get { return GetValue(BusyContentProperty); }
      set { SetValue(BusyContentProperty, value); }
    }

    public static readonly DependencyProperty OverlaySourceProperty = DependencyProperty.Register(
      "OverlaySource", typeof(object), typeof(BusyIndicatorOverlay));

    public object OverlaySource
    {
      get { return GetValue(OverlaySourceProperty); }
      set { SetValue(OverlaySourceProperty, value); }
    }
  }
}
