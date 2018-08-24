// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-24 4:35 PM

namespace eagleboost.core.Reflection
{
  using System;
  using System.Reflection;

  /// <summary>
  /// TypeExt
  /// </summary>
  public static class TypeExt
  {
    private const BindingFlags InstanceMethodFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

    public static Action<TOwner, T> CreateAction<TOwner, T>(this Type type, string methodName)
    {
      var mi = type.GetMethod(methodName, InstanceMethodFlags, Type.DefaultBinder, new[] { typeof(T) }, null);
      return (Action<TOwner, T>)Delegate.CreateDelegate(typeof(Action<TOwner, T>), null, mi);
    }

    public static Action<TOwner, T1, T2> CreateAction<TOwner, T1, T2>(this Type type, string methodName)
    {
      var mi = type.GetMethod(methodName, InstanceMethodFlags, Type.DefaultBinder, new[] { typeof(T1), typeof(T2) }, null);
      return (Action<TOwner, T1, T2>)Delegate.CreateDelegate(typeof(Action<TOwner, T1, T2>), null, mi);
    }

    public static Action<TOwner, T1, T2, T3> CreateAction<TOwner, T1, T2, T3>(this Type type, string methodName)
    {
      var mi = type.GetMethod(methodName, InstanceMethodFlags, Type.DefaultBinder, new[] { typeof(T1), typeof(T2), typeof(T3) }, null);
      return (Action<TOwner, T1, T2, T3>)Delegate.CreateDelegate(typeof(Action<TOwner, T1, T2, T3>), null, mi);
    }
  }
}