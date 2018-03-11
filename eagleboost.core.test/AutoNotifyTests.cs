using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eagleboost.core.test
{
  using System.Collections.Generic;
  using eagleboost.component.Interceptions;
  using eagleboost.core.Contracts.AutoNotify;
  using eagleboost.sharedcomponents;
  using FluentAssert;
  using Unity;
  using Unity.Interception.ContainerIntegration;
  using Unity.Interception.Interceptors.TypeInterceptors.VirtualMethodInterception;

  [TestClass]
  public class AutoNotifyTests
  {
    [TestMethod]
    public void SimpleViewModelTest()
    {
      var container = new UnityContainer();
      container.AddNewExtension<Interception>();

      container.RegisterType<ViewModel>(new Interceptor<VirtualMethodInterceptor>(),
        new InterceptionBehavior<NotifyPropertyChangedBehavior>());

      var viewModel = container.Resolve<ViewModel>();
      viewModel.FirstName.ShouldBeNull();
      viewModel.LastName.ShouldBeNull();
      viewModel.Error.ShouldBeEqualTo("FirstName cannot be empty");
      viewModel.OkCommand.CanExecute(null).ShouldBeFalse();

      var changedLog = new List<string>();
      viewModel.PropertyChanged += (s, e) => changedLog.Add(e.PropertyName);
      viewModel.OkCommand.CanExecuteChanged += (s, e) => changedLog.Add("OkCommandCanExecuteChanged");
      viewModel.FirstName = "Jason";
      changedLog.ShouldContainAllInOrder(new[] {"FirstName", "FullName", "Error", "OkCommandCanExecuteChanged"});
      viewModel.Error.ShouldBeEqualTo("LastName cannot be empty");
      viewModel.OkCommand.CanExecute(null).ShouldBeTrue();

      var invokedLog = new List<MethodInvokedEventArgs>();
      ((IAutoNotify)viewModel).MethodInvoked += (s, e) => invokedLog.Add(e);
      viewModel.Age = viewModel.Age + 1;
      invokedLog.Count.ShouldBeEqualTo(2);
      invokedLog[0].Name.ShouldBeEqualTo("OnAgeChanged");
      var context = invokedLog[0].InvokeContext;
      context.Property.ShouldBeEqualTo("Age");
      context.OldValue<int>().ShouldBeEqualTo(0);
      context.NewValue<int>().ShouldBeEqualTo(1);
      invokedLog[1].Name.ShouldBeEqualTo("OnAgeChanged2");
      invokedLog[1].InvokeContext.ShouldBeNull();
    }
  }
}
