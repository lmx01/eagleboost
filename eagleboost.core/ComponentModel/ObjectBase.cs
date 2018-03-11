// Author : Shuo Zhang
// 
// Creation :2018-03-02 22:41

namespace eagleboost.core.ComponentModel
{
  using System;
  using System.Linq.Expressions;

  public class ObjectBase
  {
    protected static string Property<T>(Expression<Func<T, object>> expr)
    {
      var member = GetMember(expr);
      return member;
    }

    protected static string GetMember(Expression expression)
    {
      if (expression == null)
        throw new ArgumentException("Getting property name form expression is not supported for this type.");

      var lamda = expression as LambdaExpression;
      if (lamda == null)
        throw new NotSupportedException("Getting property name form expression is not supported for this type.");

      var mbe = lamda.Body as MemberExpression;
      if (mbe != null)
        return mbe.Member.Name;

      var unary = lamda.Body as UnaryExpression;
      if (unary != null)
      {
        var member = unary.Operand as MemberExpression;
        if (member != null)
          return member.Member.Name;
      }

      throw new NotSupportedException("Getting property name form expression is not supported for this type.");
    }
  }
}