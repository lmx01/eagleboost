// Author : Shuo Zhang
// 
// Creation :2018-03-09 15:42

namespace eagleboost.core.ComponentModel.AutoNotify
{
  using System;
  using System.Collections.Generic;
  using System.Linq.Expressions;
  using System.Reflection;
  using eagleboost.core.Extensions;

  public abstract class AutoNotifyOperation<TOperation, T, TNotify> where TOperation : AutoNotifyOperation<TOperation, T, TNotify>
  {
    #region Statics
    public static readonly Dictionary<string, List<TNotify>> NotifyMap;
    #endregion Statics

    #region Declarations
    private readonly TNotify _toNotify;
    #endregion Declarations

    #region ctors
    static AutoNotifyOperation()
    {
      NotifyMap = new Dictionary<string, List<TNotify>>();
    }

    protected AutoNotifyOperation(TNotify toNotify)
    {
      _toNotify = toNotify;
    }
    #endregion ctors

    public TOperation By<TProperty>(params Expression<Func<T, TProperty>>[] selectors)
    {
      foreach (var selector in selectors)
      {
        var body = (MemberExpression)selector.Body;
        var sourceProperty = (PropertyInfo)body.Member;
        By(sourceProperty.Name);
      }

      return (TOperation)this;
    }

    public TOperation By(string property)
    {
      var list = NotifyMap.GetOrCreate(property);
      if (!list.Contains(_toNotify))
      {
        list.Add(_toNotify);
      }

      return (TOperation)this;
    }
  }
}