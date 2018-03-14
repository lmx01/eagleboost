using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eagleboost.core.test
{
  using System;
  using System.Collections.Generic;
  using System.Linq.Expressions;
  using eagleboost.component.Extensions;
  using eagleboost.core.ComponentModel.AutoNotify;
  using eagleboost.core.Contracts.AutoNotify;
  using eagleboost.sharedcomponents.ViewModels;
  using FluentAssert;
  using Unity;
  using Unity.Interception.ContainerIntegration;

  [TestClass]
  public class AutoNotifyTests
  {
    [TestMethod]
    public void InternalSetupTest()
    {
      var container = new UnityContainer();
      container.AddNewExtension<Interception>();

      container.RegisterAutoNotifyType<ViewModel>();

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
      ((IMethodAutoInvoked) viewModel).MethodInvoked += (s, e) => invokedLog.Add(e);
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

    [TestMethod]
    public void ExternalSetupTest()
    {
      var container = new UnityContainer();
      container.AddNewExtension<Interception>();

      container.RegisterAutoNotifyType<PureViewModel>();

      Expression<Func<PureViewModel, string>>[] namesSelector = {v => v.FirstName, v => v.LastName};

      AutoNotifySetup<PureViewModel>.Invoke(v => v.OnAgeChanged).By(v => v.Age);
      AutoNotifySetup<PureViewModel>.Invoke(v => v.OnAgeChanged2).By(v => v.Age);
      AutoNotifySetup<PureViewModel>.Notify(v => v.FullName).By(namesSelector);
      AutoNotifySetup<PureViewModel>.Notify(v => v.Error).By(namesSelector);
      AutoNotifySetup<PureViewModel>.Invalidate(v => v.OkCommand).By(namesSelector);

      var viewModel = container.Resolve<PureViewModel>();
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
      ((IMethodAutoInvoked) viewModel).MethodInvoked += (s, e) => invokedLog.Add(e);
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

    [TestMethod]
    public void ChangeArgsTest()
    {
      var args = PropertyChangeArgs<ViewModel>.Instance;

      var firstName = args.GetChangedArgs(v => v.FirstName);
      firstName.ShouldNotBeNull();
      args.GetChangedArgs(v => v.FirstName).ShouldBeEqualTo(firstName);

      var lastName = args.GetChangedArgs(v => v.LastName);
      lastName.ShouldNotBeNull();
      args.GetChangedArgs(v => v.LastName).ShouldBeEqualTo(lastName);

      var fulleName = args.GetChangedArgs(v => v.FullName);
      fulleName.ShouldNotBeNull();
      args.GetChangedArgs(v => v.FullName).ShouldBeEqualTo(fulleName);

      var age = args.GetChangedArgs(v => v.Age);
      age.ShouldNotBeNull();
      args.GetChangedArgs(v => v.Age).ShouldBeEqualTo(age);

      var error = args.GetChangedArgs(v => v.Error);
      error.ShouldNotBeNull();
      args.GetChangedArgs(v => v.Error).ShouldBeEqualTo(error);

      var okCmd = args.GetChangedArgs(v => v.OkCommand);
      okCmd.ShouldNotBeNull();
      args.GetChangedArgs(v => v.OkCommand).ShouldBeEqualTo(okCmd);
    }
  }
}