// Author : Shuo Zhang
// 
// Creation :2018-03-02 22:41

namespace eagleboost.core.ComponentModel
{
  using System;
  using System.Linq.Expressions;
  using eagleboost.core.Extensions;

  public class ObjectBase
  {
    protected static string Property<T>(Expression<Func<T, object>> expr)
    {
      var member = GetMember(expr);
      return member;
    }

    protected static string GetMember(Expression expression)
    {
      return expression.GetMember();
    }
  }
}