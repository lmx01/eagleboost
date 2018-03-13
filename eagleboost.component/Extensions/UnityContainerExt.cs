// Author : Shuo Zhang
// 
// Creation :2018-03-12 17:43

namespace eagleboost.component.Extensions
{
  using System.Collections.Generic;
  using eagleboost.component.Interceptions;
  using Unity;
  using Unity.Interception.ContainerIntegration;
  using Unity.Interception.Interceptors.TypeInterceptors.VirtualMethodInterception;
  using Unity.Lifetime;
  using Unity.Registration;

  public static class UnityContainerExt
  {
    public static IUnityContainer RegisterNotifyPropertyChangedType<T>(this IUnityContainer container, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers) where T : class
    {
      var newInjectionMembers = new List<InjectionMember>(injectionMembers)
      {
        new Interceptor<VirtualMethodInterceptor>(),
        new InterceptionBehavior<NotifyPropertyChangedBehavior<T>>()
      };
      return container.RegisterType(null, typeof(T), null, lifetimeManager, newInjectionMembers.ToArray());
    }

    public static IUnityContainer RegisterNotifyPropertyChangedType<T>(this IUnityContainer container, params InjectionMember[] injectionMembers) where T : class
    {
      var newInjectionMembers = new List<InjectionMember>(injectionMembers)
      {
        new Interceptor<VirtualMethodInterceptor>(),
        new InterceptionBehavior<NotifyPropertyChangedBehavior<T>>()
      };
      return container.RegisterType(null, typeof(T), null, null, newInjectionMembers.ToArray());
    }
  }
}