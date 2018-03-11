using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace eagleboost.sampleapp
{
  using eagleboost.component.Interceptions;
  using eagleboost.presentation.Behaviors;
  using eagleboost.sharedcomponents;
  using Unity;
  using Unity.Interception.ContainerIntegration;
  using Unity.Interception.Interceptors.TypeInterceptors.VirtualMethodInterception;

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

      // todo: Register an interface instead of a type.
      container.RegisterType<ViewModel>(new Interceptor<VirtualMethodInterceptor>(),
        new InterceptionBehavior<NotifyPropertyChangedBehavior>());
      var viewModel = container.Resolve<ViewModel>();
      DataContext = viewModel;
      TextBoxSelectAll.Install();
    }
  }
}
