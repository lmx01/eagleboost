// Author : Shuo Zhang
// 
// Creation :2018-03-09 15:29

namespace eagleboost.core.ComponentModel.AutoNotify
{
  using System.Linq.Expressions;
  using System.Reflection;

  public class InvalidateBy<T> : AutoNotifyOperation<InvalidateBy<T>, T, string>
  {
    public InvalidateBy(string toNotify) : base(toNotify)
    {
    }

    public InvalidateBy(PropertyInfo toNotify) : base(toNotify.Name)
    {
    }

    public InvalidateBy(MemberExpression memberExpr) : this((PropertyInfo)memberExpr.Member)
    {
    }
  }
}