// Author : Shuo Zhang
// 
// Creation :2018-03-01 19:49

namespace eagleboost.component.Interceptions
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Reflection;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.ComponentModel.AutoNotify;
  using eagleboost.core.Contracts;
  using eagleboost.core.Contracts.AutoNotify;
  using eagleboost.core.Exceptions;
  using eagleboost.core.Extensions;
  using Unity.Interception.InterceptionBehaviors;
  using Unity.Interception.PolicyInjection.Pipeline;

  public class AutoNotifyBehavior<T> : IInterceptionBehavior where T : class, IAutoNotify
  {
    #region Declarations
    private event PropertyChangingEventHandler PropertyChanging;
    private event PropertyChangedEventHandler PropertyChanged;
    private Dictionary<string, List<string>> _changeWithMap;
    private Dictionary<string, List<MethodInfo>> _invokeWithMap;
    private Dictionary<string, List<IInvalidatableCommand>> _invalidateWithMap;
    private IPropertyChangeArgs _propertyChangeArgs;
    #endregion Declarations

    #region IInterceptionBehavior
    public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
    {
      EnsureType(input);
      EnsureChangeWithMap(input);
      EnsureInvokeWithMap(input);
      EnsureInvalidateWithMap(input);

      if (input.MethodBase.Name == NotifyPropertyChangeInfo.AddPropertyChangingMethodName)
      {
        return AddPropertyChangingHandler(input);
      }

      if (input.MethodBase.Name == NotifyPropertyChangeInfo.RemovePropertyChangingMethodName)
      {
        return RemovePropertyChangingHandler(input);
      }

      if (input.MethodBase.Name== NotifyPropertyChangeInfo.AddPropertyChangedMethodName)
      {
        return AddPropertyChangedHandler(input);
      }

      if (input.MethodBase.Name == NotifyPropertyChangeInfo.RemovePropertyChangedMethodName)
      {
        return RemovePropertyChangedHandler(input);
      }

      if (input.MethodBase.Name == NotifyPropertyChangeInfo.NotifyPropertyChangedMethodName)
      {
        return InterceptNotifyPropertyChanged(input, getNext);
      }

      if (input.MethodBase.Name == NotifyPropertyChangeInfo.NotifyPropertyChangingMethodName)
      {
        return InterceptNotifyPropertyChanging(input, getNext);
      }

      if (IsPropertySetter(input))
      {
        return InterceptPropertySet(input, getNext);
      }

      return getNext()(input, getNext);
    }

    /// <summary>
    /// Optimization hint for proxy generation - will this behavior actually
    /// perform any operations when invoked?
    /// </summary>
    public bool WillExecute
    {
      get { return true; }
    }

    /// <summary>
    /// Returns the interfaces required by the behavior for the objects it intercepts.
    /// </summary>
    /// <returns>
    /// The required interfaces.
    /// </returns>
    public IEnumerable<Type> GetRequiredInterfaces()
    {
      return new[] { typeof(IAutoNotify) };
    }
    #endregion IInterceptionBehavior

    #region Virtuals

    protected virtual Type GetTargetType(IMethodInvocation input)
    {
      return input.MethodBase.DeclaringType;
    }
    #endregion Virtuals

    #region Private Methods
    private IMethodReturn AddPropertyChangedHandler(IMethodInvocation input)
    {
      var handler = (PropertyChangedEventHandler)input.Arguments[0];
      PropertyChanged += handler;
      return input.CreateMethodReturn(null);
    }

    private IMethodReturn RemovePropertyChangedHandler(IMethodInvocation input)
    {
      var handler = (PropertyChangedEventHandler)input.Arguments[0];
      PropertyChanged -= handler;
      return input.CreateMethodReturn(null);
    }

    private IMethodReturn AddPropertyChangingHandler(IMethodInvocation input)
    {
      var handler = (PropertyChangingEventHandler)input.Arguments[0];
      PropertyChanging += handler;
      return input.CreateMethodReturn(null);
    }

    private IMethodReturn RemovePropertyChangingHandler(IMethodInvocation input)
    {
      var handler = (PropertyChangingEventHandler)input.Arguments[0];
      PropertyChanging -= handler;
      return input.CreateMethodReturn(null);
    }

    private static bool IsPropertySetter(IMethodInvocation input)
    {
      return input.MethodBase.IsSpecialName && input.MethodBase.Name.StartsWith("set_");
    }

    private void EnsureType(IMethodInvocation input)
    {
      if (!typeof(T).IsCompatiableWith<IAutoNotify>())
      {
        throw InvalidTypeException.Create<T>();
      }
    }

    private void EnsureChangeWithMap(IMethodInvocation input)
    {
      if (_changeWithMap != null)
      {
        return;
      }

      var autoNotify = (IAutoNotify)input.Target;
      _changeWithMap = autoNotify.Config.NotifyMap ?? new Dictionary<string, List<string>>();
    }

    private void EnsureInvokeWithMap(IMethodInvocation input)
    {
      if (_invokeWithMap != null)
      {
        return;
      }

      var autoNotify = (IAutoNotify)input.Target;
      _invokeWithMap = autoNotify.Config.InvokeMap ?? new Dictionary<string, List<MethodInfo>>();
    }

    private void EnsureInvalidateWithMap(IMethodInvocation input)
    {
      if (_invalidateWithMap != null)
      {
        return;
      }

      _invalidateWithMap = new Dictionary<string, List<IInvalidatableCommand>>();

      var autoNotify = (IAutoNotify)input.Target;
      var invalidateWithMap = autoNotify.Config.InvalidateMap;

      if (invalidateWithMap != null)
      {
        foreach (var p in invalidateWithMap)
        {
          var sourceProperty = p.Key;
          var targetPrpoerties = p.Value;
          foreach (var targetProperty in targetPrpoerties)
          {
            var chain = _invalidateWithMap.GetOrCreate(sourceProperty);
            var property = input.Target.GetType().GetProperty(targetProperty);
            var command = (IInvalidatableCommand) property.GetValue(input.Target);
            chain.Add(command);
          }
        }
      }
    }

    private IMethodReturn InterceptPropertySet(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
    {
      IMethodReturn result;
      var propertyName = input.MethodBase.Name.Substring(4);
      var objectType = input.MethodBase.DeclaringType;
      var getMethod =  objectType.GetMethod(input.MethodBase.Name.Replace("set_", "get_"));
      var oldValue = getMethod.Invoke(input.Target, null);
      var newVlaue = input.Arguments[0];
      if (!Equals(oldValue, newVlaue))
      {
        var changingHandler = PropertyChanging;
        if (changingHandler != null)
        {
          var changingArgs = GetChangingArgs(input, propertyName);
          changingHandler(input.Target, changingArgs);
        }

        var extChangeNotify = input.Target as IExternalPropertyChangeNotify;
        if (extChangeNotify != null)
        {
          extChangeNotify.OnPropertyChanging(propertyName);
        }

        result = getNext()(input, getNext);

        if (extChangeNotify != null)
        {
          extChangeNotify.OnPropertyChanged(propertyName);
        }

        var changedHandler = PropertyChanged;
        if (changedHandler != null)
        {
          var changedArgs = GetChangedArgs(input, propertyName);
          changedHandler(input.Target, changedArgs);

          List<string> propertyChain;
          if (_changeWithMap.TryGetValue(propertyName, out propertyChain))
          {
            foreach (var p in propertyChain)
            {
              var args = GetChangedArgs(input, p);
              changedHandler(input.Target, args);
            }
          }

          List<MethodInfo> methodChain;
          if (_invokeWithMap.TryGetValue(propertyName, out methodChain))
          {
            var methodInvoked = input.Target as IMethodAutoInvoked;
            foreach (var method in methodChain)
            {
              var parameters = method.GetParameters();
              if (parameters.Length == 0)
              {
                method.Invoke(input.Target, new object[0]);
                if (methodInvoked != null)
                {
                  methodInvoked.OnMethodInvoked(method.Name, null);
                }
              }
              else if (parameters[0].ParameterType.IsCompatiableWith<InvokeContext>())
              {
                var invokeContext = InvokeContext.Create(propertyName, oldValue, newVlaue);
                method.Invoke(input.Target, new object[] {invokeContext});
                if (methodInvoked != null)
                {
                  methodInvoked.OnMethodInvoked(method.Name, invokeContext);
                }
              }
            }
          }

          List<IInvalidatableCommand> commandChain;
          if (_invalidateWithMap.TryGetValue(propertyName, out commandChain))
          {
            foreach (var method in commandChain)
            {
              method.Invalidate();
            }
          }
        }
      }
      else
      {
        result = getNext()(input, getNext);
      }

      return result;
    }

    private IMethodReturn InterceptNotifyPropertyChanged(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
    {
      var result = getNext()(input, getNext);

      var changedHandler = PropertyChanged;
      if (changedHandler != null)
      {
        var propertyName = (string) input.Arguments[0];
        var changedArgs = GetChangedArgs(input, propertyName);
        changedHandler(input.Target, changedArgs);
      }

      return result;
    }

    private IMethodReturn InterceptNotifyPropertyChanging(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
    {
      var changingHandler = PropertyChanging;
      if (changingHandler != null)
      {
        var propertyName = (string)input.Arguments[0];
        var changingArgs = GetChangingArgs(input, propertyName);
        changingHandler(input.Target, changingArgs);
      }

      var result = getNext()(input, getNext);
      return result;
    }

    private IPropertyChangeArgs PropertyChangeArgs
    {
      get
      {
        if (_propertyChangeArgs == null)
        {
          var type = typeof(PropertyChangeArgs<>).MakeGenericType(typeof(T));
          var propertyChangeArgs = (IPropertyChangeArgs) Activator.CreateInstance(type);
          _propertyChangeArgs = propertyChangeArgs;
        }

        return _propertyChangeArgs;
      }
    }

    private PropertyChangedEventArgs GetChangedArgs(IMethodInvocation input, string p)
    {
      return PropertyChangeArgs.GetChangedArgs(p);
    }

    private PropertyChangingEventArgs GetChangingArgs(IMethodInvocation input, string p)
    {
      return PropertyChangeArgs.GetChangingArgs(p);
    }
    #endregion Private Methods
  }
}