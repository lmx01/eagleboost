﻿using System;
using System.Windows;

namespace eagleboost.sampleapp
{
  using eagleboost.presentation.Behaviors;
  using eagleboost.sharedcomponents.ViewModels;
  using eagleboost.shell.Tools;
  using Unity;
  using Unity.Interception.ContainerIntegration;

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      var container = new UnityContainer();
      container.AddNewExtension<Interception>();

      //// todo: Register an interface instead of a type.
      //container.RegisterAutoNotifyType<PureViewModel>();

      //Expression<Func<PureViewModel, string>>[] namesSelector = { v => v.FirstName, v => v.LastName };

      //AutoNotifySetup<PureViewModel>.Invoke(v => v.OnAgeChanged).By(v => v.Age);
      //AutoNotifySetup<PureViewModel>.Invoke(v => v.OnAgeChanged2).By(v => v.Age);
      //AutoNotifySetup<PureViewModel>.Notify(v => v.FullName).By(namesSelector);
      //AutoNotifySetup<PureViewModel>.Notify(v => v.Error).By(namesSelector);
      //AutoNotifySetup<PureViewModel>.Invalidate(v => v.OkCommand).By(namesSelector);

      var a = new ApplicationActivationTracker();
      a.Start();
      var viewModel = container.Resolve<PureViewModel>();
      DataContext = viewModel;
      TextBoxSelectAll.Install();

      ClipboardMonitor.Install();
      ClipboardMonitor.ClipboardUpdated += ClipboardMonitor_ClipboardUpdated;
      //DispatcherViewFactory.InvokeAsync("ABC", () =>
      //{
      //  var w = new ViewControllerWindow { Title = "ABC" ,Content = "I'm running on thead 'ABC'",
      //    ShowInTaskbar = true,
      //    Topmost = true,
      //  }.RemoveIcon().HideMinMaxButton(); ;
      //  w.Show();
      //});
      Closed += MainWindow_Closed;
    }

    private void MainWindow_Closed(object sender, EventArgs e)
    {
      ClipboardMonitor.ClipboardUpdated -= ClipboardMonitor_ClipboardUpdated;
      ClipboardMonitor.Uninstall();
    }

    private void ClipboardMonitor_ClipboardUpdated(object sender, ClipboardChangedEventArgs args)
    {
    }
  }
}
