// Author : Shuo Zhang
// 
// Creation :2018-03-12 17:33

namespace eagleboost.core.ComponentModel.AutoNotify
{
  using System;
  using System.Linq.Expressions;
  using System.Reflection;

  public class AutoNotifySetup<T> where T : class
  {
    #region Protected Methods
    public static NotifyBy<T> Notify<TProperty>(Expression<Func<T, TProperty>> selector)
    {
      AutoNotifyConfig<T>.Instance.SetNotifyMap(NotifyBy<T>.NotifyMap);

      return new NotifyBy<T>((MemberExpression)selector.Body);
    }

    public static InvalidateBy<T> Invalidate<TProperty>(Expression<Func<T, TProperty>> selector)
    {
      AutoNotifyConfig<T>.Instance.SetInvalidateMap(InvalidateBy<T>.NotifyMap);

      return new InvalidateBy<T>((MemberExpression)selector.Body);
    }

    public static InvokeBy<T> Invoke(Expression<Func<T, Action<InvokeContext>>> selector)
    {
      AutoNotifyConfig<T>.Instance.SetInvokeMap(InvokeBy<T>.NotifyMap);

      var unaryExpression = (UnaryExpression)selector.Body;
      var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
      var methodCallObject = (ConstantExpression)methodCallExpression.Object;
      var methodInfo = (MethodInfo)methodCallObject.Value;
      return new InvokeBy<T>(methodInfo);
    }

    public static InvokeBy<T> Invoke(Expression<Func<T, Action>> selector)
    {
      AutoNotifyConfig<T>.Instance.SetInvokeMap(InvokeBy<T>.NotifyMap);

      var unaryExpression = (UnaryExpression)selector.Body;
      var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
      var methodCallObject = (ConstantExpression)methodCallExpression.Object;
      var methodInfo = (MethodInfo)methodCallObject.Value;
      return new InvokeBy<T>(methodInfo);
    }
    #endregion Protected Methods
  }
}