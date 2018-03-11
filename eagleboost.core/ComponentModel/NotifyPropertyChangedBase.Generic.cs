// Author : Shuo Zhang
// 
// Creation :2018-03-06 16:48

namespace eagleboost.core.ComponentModel
{
  using System;
  using System.Linq.Expressions;
  using System.Reflection;
  using eagleboost.core.ComponentModel.AutoNotify;
  using eagleboost.core.Contracts.AutoNotify;

  /// <summary>
  /// NotifyPropertyChangedBase
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class NotifyPropertyChangedBase<T> : NotifyPropertyChangedBase, IAutoNotify, IMethodInvoked where T : class
  {
    #region Declarations
    private event MethodInvokedEventHandler MethodInvokedHandler;
    #endregion Declarations

    #region Protected Methods
    protected static NotifyBy<T> Notify<TProperty>(Expression<Func<T, TProperty>> selector)
    {
      AutoNotifyConfig<T>.Instance.SetNotifyMap(NotifyBy<T>.NotifyMap);

      return new NotifyBy<T>((MemberExpression)selector.Body);
    }

    protected static InvalidateBy<T> Invalidate<TProperty>(Expression<Func<T, TProperty>> selector)
    {
      AutoNotifyConfig<T>.Instance.SetInvalidateMap(InvalidateBy<T>.NotifyMap);

      return new InvalidateBy<T>((MemberExpression)selector.Body);
    }

    protected static InvokeBy<T> Invoke(Expression<Func<T, Action<InvokeContext>>> selector)
    {
      AutoNotifyConfig<T>.Instance.SetInvokeMap(InvokeBy<T>.NotifyMap);

      var unaryExpression = (UnaryExpression)selector.Body;
      var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
      var methodCallObject = (ConstantExpression)methodCallExpression.Object;
      var methodInfo = (MethodInfo)methodCallObject.Value;
      return new InvokeBy<T>(methodInfo);
    }

    protected static InvokeBy<T> Invoke(Expression<Func<T, Action>> selector)
    {
      AutoNotifyConfig<T>.Instance.SetInvokeMap(InvokeBy<T>.NotifyMap);

      var unaryExpression = (UnaryExpression)selector.Body;
      var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
      var methodCallObject = (ConstantExpression)methodCallExpression.Object;
      var methodInfo = (MethodInfo)methodCallObject.Value;
      return new InvokeBy<T>(methodInfo);
    }
    #endregion Protected Methods

    #region IAutoNotify
    public IAutoNotifyConfig Config
    {
      get { return AutoNotifyConfig<T>.Instance; }
    }

    event MethodInvokedEventHandler IAutoNotify.MethodInvoked
    {
      add { MethodInvokedHandler += value; }
      remove { MethodInvokedHandler -= value; }
    }

    #endregion IAutoNotify

    #region IMethodInvoked
    void IMethodInvoked.OnMethodInvoked(string name, InvokeContext context)
    {
      var handler = MethodInvokedHandler;
      if (handler != null)
      {
        handler(this, new MethodInvokedEventArgs(name, context));
      }
    }
    #endregion IMethodInvoked
  }
}