namespace eagleboost.sampleapp.SelectionContainerSample
{
  using System;
  using System.Collections.ObjectModel;
  using System.Windows.Controls;
  using eagleboost.core.Collections;
  using eagleboost.core.ComponentModel;

  /// <summary>
  /// Interaction logic for SelectionContainerView.xaml
  /// </summary>
  public partial class SelectionContainerView : UserControl
  {
    public SelectionContainerView()
    {
      InitializeComponent();
      DataContext = new SelectionContainerViewModel();
    }
  }
}
