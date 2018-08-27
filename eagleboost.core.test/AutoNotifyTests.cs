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
  using FluentAssertions;
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
      viewModel.FirstName.Should().BeNull();
      viewModel.LastName.Should().BeNull();
      viewModel.Error.Should().Be("FirstName cannot be empty");
      viewModel.OkCommand.CanExecute(null).Should().BeFalse();

      var changedLog = new List<string>();
      viewModel.PropertyChanged += (s, e) => changedLog.Add(e.PropertyName);
      viewModel.OkCommand.CanExecuteChanged += (s, e) => changedLog.Add("OkCommandCanExecuteChanged");
      viewModel.FirstName = "Jason";
      changedLog.Should().BeEquivalentTo(new[] {"FirstName", "FullName", "Error", "OkCommandCanExecuteChanged"});
      viewModel.Error.Should().Be("LastName cannot be empty");
      viewModel.OkCommand.CanExecute(null).Should().BeTrue();

      var invokedLog = new List<MethodInvokedEventArgs>();
      ((IMethodAutoInvoked) viewModel).MethodInvoked += (s, e) => invokedLog.Add(e);
      viewModel.Age = viewModel.Age + 1;
      invokedLog.Count.Should().Be(2);
      invokedLog[0].Name.Should().Be("OnAgeChanged");
      var context = invokedLog[0].InvokeContext;
      context.Property.Should().Be("Age");
      context.OldValue<int>().Should().Be(0);
      context.NewValue<int>().Should().Be(1);
      invokedLog[1].Name.Should().Be("OnAgeChanged2");
      invokedLog[1].InvokeContext.Should().BeNull();
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
      viewModel.FirstName.Should().BeNull();
      viewModel.LastName.Should().BeNull();
      viewModel.Error.Should().Be("FirstName cannot be empty");
      viewModel.OkCommand.CanExecute(null).Should().BeFalse();

      var changedLog = new List<string>();
      viewModel.PropertyChanged += (s, e) => changedLog.Add(e.PropertyName);
      viewModel.OkCommand.CanExecuteChanged += (s, e) => changedLog.Add("OkCommandCanExecuteChanged");
      viewModel.FirstName = "Jason";
      changedLog.Should().BeEquivalentTo(new []{"FirstName", "FullName", "Error", "OkCommandCanExecuteChanged"});
      viewModel.Error.Should().Be("LastName cannot be empty");
      viewModel.OkCommand.CanExecute(null).Should().BeTrue();

      var invokedLog = new List<MethodInvokedEventArgs>();
      ((IMethodAutoInvoked) viewModel).MethodInvoked += (s, e) => invokedLog.Add(e);
      viewModel.Age = viewModel.Age + 1;
      invokedLog.Count.Should().Be(2);
      invokedLog[0].Name.Should().Be("OnAgeChanged");
      var context = invokedLog[0].InvokeContext;
      context.Property.Should().Be("Age");
      context.OldValue<int>().Should().Be(0);
      context.NewValue<int>().Should().Be(1);
      invokedLog[1].Name.Should().Be("OnAgeChanged2");
      invokedLog[1].InvokeContext.Should().BeNull();
    }

    [TestMethod]
    public void SelfNotifyTest()
    {
      var container = new UnityContainer();
      container.AddNewExtension<Interception>();

      container.RegisterAutoNotifyType<ViewModel>();

      var viewModel = container.Resolve<ViewModel>();
      var changedLog = new List<string>();
      var changingLog = new List<string>();
      viewModel.PropertyChanged += (s, e) => changedLog.Add(e.PropertyName);
      viewModel.PropertyChanging += (s, e) => changingLog.Add(e.PropertyName);
      viewModel.Age = viewModel.Age + 1;
      changedLog.Count.Should().Be(2);
      changedLog[0].Should().Be("Age");
      changedLog[1].Should().Be("FullNameSelfNotify");
      changingLog.Count.Should().Be(1);
      changingLog[0].Should().Be("Age");
    }

    [TestMethod]
    public void ChangeArgsTest()
    {
      var args = PropertyChangeArgs<ViewModel>.Instance;

      var firstName = args.GetChangedArgs(v => v.FirstName);
      firstName.Should().NotBeNull();
      args.GetChangedArgs(v => v.FirstName).Should().Be(firstName);

      var lastName = args.GetChangedArgs(v => v.LastName);
      lastName.Should().NotBeNull();
      args.GetChangedArgs(v => v.LastName).Should().Be(lastName);

      var fulleName = args.GetChangedArgs(v => v.FullName);
      fulleName.Should().NotBeNull();
      args.GetChangedArgs(v => v.FullName).Should().Be(fulleName);

      var age = args.GetChangedArgs(v => v.Age);
      age.Should().NotBeNull();
      args.GetChangedArgs(v => v.Age).Should().Be(age);

      var error = args.GetChangedArgs(v => v.Error);
      error.Should().NotBeNull();
      args.GetChangedArgs(v => v.Error).Should().Be(error);

      var okCmd = args.GetChangedArgs(v => v.OkCommand);
      okCmd.Should().NotBeNull();
      args.GetChangedArgs(v => v.OkCommand).Should().Be(okCmd);
    }
  }
}