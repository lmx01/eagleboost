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
  using System.Linq.Expressions;
  using eagleboost.component.Extensions;
  using eagleboost.component.Interceptions;
  using eagleboost.core.ComponentModel.AutoNotify;
  using eagleboost.presentation.Behaviors;
  using eagleboost.sharedcomponents;
  using eagleboost.sharedcomponents.ViewModels;
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
      container.RegisterNotifyPropertyChangedType<PureViewModel>();

      Expression<Func<PureViewModel, string>>[] namesSelector = { v => v.FirstName, v => v.LastName };

      AutoNotifySetup<PureViewModel>.Invoke(v => v.OnAgeChanged).By(v => v.Age);
      AutoNotifySetup<PureViewModel>.Invoke(v => v.OnAgeChanged2).By(v => v.Age);
      AutoNotifySetup<PureViewModel>.Notify(v => v.FullName).By(namesSelector);
      AutoNotifySetup<PureViewModel>.Notify(v => v.Error).By(namesSelector);
      AutoNotifySetup<PureViewModel>.Invalidate(v => v.OkCommand).By(namesSelector);

      var viewModel = container.Resolve<PureViewModel>();
      DataContext = viewModel;
      TextBoxSelectAll.Install();
    }
  }
}
