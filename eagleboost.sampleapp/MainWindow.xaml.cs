using System;
using System.Windows;

namespace eagleboost.sampleapp
{
  using System.Linq.Expressions;
  using eagleboost.component.Extensions;
  using eagleboost.core.ComponentModel.AutoNotify;
  using eagleboost.presentation.Behaviors;
  using eagleboost.sharedcomponents.ViewModels;
  using eagleboost.UserExperience.Threading;
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
      UiThread.Initialize();

      var container = new UnityContainer();
      container.AddNewExtension<Interception>();

      // todo: Register an interface instead of a type.
      container.RegisterAutoNotifyType<PureViewModel>();

      Expression<Func<PureViewModel, string>>[] namesSelector = { v => v.FirstName, v => v.LastName };

      AutoNotifySetup<PureViewModel>.Invoke(v => v.OnAgeChanged).By(v => v.Age);
      AutoNotifySetup<PureViewModel>.Invoke(v => v.OnAgeChanged2).By(v => v.Age);
      AutoNotifySetup<PureViewModel>.Notify(v => v.FullName).By(namesSelector);
      AutoNotifySetup<PureViewModel>.Notify(v => v.Error).By(namesSelector);
      AutoNotifySetup<PureViewModel>.Invalidate(v => v.OkCommand).By(namesSelector);

      var viewModel = container.Resolve<PureViewModel>();
      DataContext = viewModel;
      TextBoxSelectAll.Install();

      DispatcherViewFactory.InvokeAsync("ABC", () =>
      {
        var w = new Window { Title = "ABC" ,Content = "I'm running on thead 'ABC'"};
        w.Show();
      });
    }
  }
}
