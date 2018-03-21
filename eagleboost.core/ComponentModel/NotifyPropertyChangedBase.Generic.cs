// Author : Shuo Zhang
// 
// Creation :2018-03-06 16:48

namespace eagleboost.core.ComponentModel
{
  using System;
  using System.ComponentModel;
  using System.Linq.Expressions;
  using eagleboost.core.ComponentModel.AutoNotify;
  using eagleboost.core.Contracts.AutoNotify;

  /// <summary>
  /// NotifyPropertyChangedBase
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class NotifyPropertyChangedBase<T> : NotifyPropertyChangedBase, IAutoNotify, IMethodAutoInvoked where T : class
  {
    #region Declarations
    private event MethodInvokedEventHandler MethodInvokedHandler;
    #endregion Declarations

    #region Protected Methods
    protected static PropertyChangedEventArgs GetChangedArgs(string propertyName)
    {
      return PropertyChangeArgs<T>.Instance.GetChangedArgs(propertyName);
    }

    protected static PropertyChangingEventArgs GetChangingArgs(string propertyName)
    {
      return PropertyChangeArgs<T>.Instance.GetChangingArgs(propertyName);
    }

    protected static PropertyChangedEventArgs GetChangedArgs(Expression<Func<T, object>> expr)
    {
      return PropertyChangeArgs<T>.Instance.GetChangedArgs(expr);
    }

    protected static PropertyChangingEventArgs GetChangingArgs(Expression<Func<T, object>> expr)
    {
      return PropertyChangeArgs<T>.Instance.GetChangingArgs(expr);
    }

    protected static NotifyBy<T> Notify<TProperty>(Expression<Func<T, TProperty>> selector)
    {
      return AutoNotifySetup<T>.Notify(selector);
    }

    protected static InvalidateBy<T> Invalidate<TProperty>(Expression<Func<T, TProperty>> selector)
    {
      return AutoNotifySetup<T>.Invalidate(selector);
    }

    protected static InvokeBy<T> Invoke(Expression<Func<T, Action<InvokeContext>>> selector)
    {
      return AutoNotifySetup<T>.Invoke(selector);
    }

    protected static InvokeBy<T> Invoke(Expression<Func<T, Action>> selector)
    {
      return AutoNotifySetup<T>.Invoke(selector);
    }
    #endregion Protected Methods

    #region IAutoNotify
    public IAutoNotifyConfig Config
    {
      get { return AutoNotifyConfig<T>.Instance; }
    }
    #endregion IAutoNotify

    #region IMethodAutoInvoked
    void IMethodAutoInvoked.OnMethodInvoked(string name, InvokeContext context)
    {
      var handler = MethodInvokedHandler;
      if (handler != null)
      {
        handler(this, new MethodInvokedEventArgs(name, context));
      }
    }

    event MethodInvokedEventHandler IMethodAutoInvoked.MethodInvoked
    {
      add { MethodInvokedHandler += value; }
      remove { MethodInvokedHandler -= value; }
    }
    #endregion IMethodAutoInvoked
  }
}