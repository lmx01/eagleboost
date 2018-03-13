// Author : Shuo Zhang
// 
// Creation :2018-03-12 17:56

namespace eagleboost.core.ComponentModel.AutoNotify
{
  using System;
  using System.Collections.Concurrent;
  using System.ComponentModel;
  using System.Linq.Expressions;
  using eagleboost.core.Contracts;
  using eagleboost.core.Extensions;

  public class PropertyChangeArgs<T> : IPropertyChangeArgs
  {
    #region Declarations
    private readonly ConcurrentDictionary<string, PropertyChangedEventArgs> _changedArgs = new ConcurrentDictionary<string, PropertyChangedEventArgs>();
    private readonly ConcurrentDictionary<string, PropertyChangingEventArgs> _changingArgs = new ConcurrentDictionary<string, PropertyChangingEventArgs>();
    #endregion Declarations

    #region ctors
    public static readonly PropertyChangeArgs<T> Instance;

    static PropertyChangeArgs()
    {
      Instance = new PropertyChangeArgs<T>();
    }
    #endregion ctors

    #region IPropertyChangeArgs
    public PropertyChangedEventArgs GetChangedArgs(string propertyName)
    {
      return _changedArgs.GetOrAdd(propertyName, p => new PropertyChangedEventArgs(p));
    }

    public PropertyChangingEventArgs GetChangingArgs(string propertyName)
    {
      return _changingArgs.GetOrAdd(propertyName, p => new PropertyChangingEventArgs(p));
    }
    #endregion IPropertyChangeArgs

    #region Public Methods
    public PropertyChangedEventArgs GetChangedArgs(Expression<Func<T, object>> expr)
    {
      var member = expr.GetMember();
      return GetChangedArgs(member);
    }

    public PropertyChangingEventArgs GetChangingArgs(Expression<Func<T, object>> expr)
    {
      var member = expr.GetMember();
      return GetChangingArgs(member);
    }
    #endregion Public Methods
  }
}