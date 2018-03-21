// Author : Shuo Zhang
// 
// Creation :2018-03-12 17:43

namespace eagleboost.component.Extensions
{
  using System;
  using System.Collections.Generic;
  using eagleboost.component.Interceptions;
  using eagleboost.core.Contracts.AutoComposite;
  using eagleboost.core.Contracts.AutoNotify;
  using Unity;
  using Unity.Interception.ContainerIntegration;
  using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;
  using Unity.Interception.Interceptors.TypeInterceptors.VirtualMethodInterception;
  using Unity.Interception.PolicyInjection;
  using Unity.Lifetime;
  using Unity.Registration;

  public static class UnityContainerExt
  {
    public static IUnityContainer RegisterAutoNotifyType<T>(this IUnityContainer container, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers) where T : class, IAutoNotify
    {
      var newInjectionMembers = new List<InjectionMember>(injectionMembers)
      {
        new Interceptor<VirtualMethodInterceptor>(),
        new InterceptionBehavior<AutoNotifyBehavior<T>>()
      };
      return container.RegisterType(null, typeof(T), null, lifetimeManager, newInjectionMembers.ToArray());
    }

    public static IUnityContainer RegisterAutoNotifyType<T>(this IUnityContainer container, params InjectionMember[] injectionMembers) where T : class, IAutoNotify
    {
      var newInjectionMembers = new List<InjectionMember>(injectionMembers)
      {
        new Interceptor<VirtualMethodInterceptor>(),
        new InterceptionBehavior<AutoNotifyBehavior<T>>()
      };
      return container.RegisterType(null, typeof(T), null, null, newInjectionMembers.ToArray());
    }

    public static IUnityContainer RegisterAutoNotifyType<TFrom, T>(this IUnityContainer container, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers) where TFrom : class, IAutoNotify where T : class, TFrom
    {
      if (!typeof(TFrom).IsInterface || typeof(T).IsInterface)
      {
        throw new ArgumentException("TFrom is not interface or T is interface");
      }
      
      var newInjectionMembers = new List<InjectionMember>(injectionMembers)
      {
        new Interceptor<InterfaceInterceptor>(),
        new InterceptionBehavior<PolicyInjectionBehavior>(),
        new Interceptor<VirtualMethodInterceptor>(),
        new InterceptionBehavior<AutoNotifyBehavior<T>>()
      };
      return container.RegisterType(typeof(TFrom), typeof(T), null, lifetimeManager, newInjectionMembers.ToArray());
    }

    public static IUnityContainer RegisterAutoNotifyType<TFrom, T>(this IUnityContainer container, params InjectionMember[] injectionMembers) where TFrom : class, IAutoNotify where T : class, TFrom
    {
      if (!typeof(TFrom).IsInterface || typeof(T).IsInterface)
      {
        throw new ArgumentException("TFrom is not interface or T is interface");
      }

      var newInjectionMembers = new List<InjectionMember>(injectionMembers)
      {
        new Interceptor<InterfaceInterceptor>(),
        new InterceptionBehavior<PolicyInjectionBehavior>(),
        new Interceptor<VirtualMethodInterceptor>(),
        new InterceptionBehavior<AutoNotifyBehavior<T>>()
      };
      return container.RegisterType(typeof(TFrom), typeof(T), null, null, newInjectionMembers.ToArray());
    }

    public static IUnityContainer RegisterAutoCompositeType<TIntf, T>(this IUnityContainer container, params InjectionMember[] injectionMembers) where T : class, TIntf, IAutoComposite<TIntf>
    {
      if (typeof(T).IsInterface)
      {
        throw new ArgumentException("TFrom is not interface or T is interface");
      }

      var newInjectionMembers = new List<InjectionMember>(injectionMembers)
      {
        new Interceptor<VirtualMethodInterceptor>(),
        new InterceptionBehavior<AutoCompositeBehavior<TIntf, T>>()
      };
      return container.RegisterType(null, typeof(T), null, null, newInjectionMembers.ToArray());
    }
  }
}