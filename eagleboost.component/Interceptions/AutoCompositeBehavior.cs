// Author : Shuo Zhang
// 
// Creation :2018-03-16 14:31

namespace eagleboost.component.Interceptions
{
  using System;
  using System.Collections.Generic;
  using System.Linq.Expressions;
  using eagleboost.core.Contracts.AutoComposite;
  using Unity.Interception.InterceptionBehaviors;
  using Unity.Interception.PolicyInjection.Pipeline;

  public class AutoCompositeBehavior<TIntf, T> : IInterceptionBehavior where T : IAutoComposite<TIntf>, TIntf
  {
    #region IInterceptionBehavior
    public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
    {
      if (IsPropertyGetter(input))
      {
        return InterceptPropertyGet(input, getNext);
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
      return new[] { typeof(IAutoComposite<TIntf>) };
    }
    #endregion IInterceptionBehavior

    #region Virtuals

    protected virtual Type GetTargetType(IMethodInvocation input)
    {
      return input.MethodBase.DeclaringType;
    }
    #endregion Virtuals

    #region Private Methods
    private static bool IsPropertyGetter(IMethodInvocation input)
    {
      return input.MethodBase.IsSpecialName && input.MethodBase.Name.StartsWith("get_");
    }

    private IMethodReturn InterceptPropertyGet(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
    {
      var compositeSource = ((IAutoComposite<TIntf>) input.Target).CompositeSource;
      var param = Expression.Parameter(typeof(TIntf));
      var getter = Expression.Lambda<Func<TIntf, object>>(Expression.Convert(
        Expression.Property(param, input.MethodBase.Name.Replace("get_", "")), typeof(object)), param).Compile();
      var result = getter(compositeSource);
      return input.CreateMethodReturn(result);
    }
    #endregion Private Methods
  }
}