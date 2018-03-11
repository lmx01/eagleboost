// Author : Shuo Zhang
// 
// Creation :2018-03-06 15:39

namespace eagleboost.core.ComponentModel.AutoNotify
{
  using System.Linq.Expressions;
  using System.Reflection;

  public class NotifyBy<T> : AutoNotifyOperation<NotifyBy<T>, T, string>
  {
    public NotifyBy(string toNotify) : base(toNotify)
    {
    }

    public NotifyBy(PropertyInfo toNotify) : base(toNotify.Name)
    {
    }

    public NotifyBy(MemberExpression memberExpr) : this((PropertyInfo)memberExpr.Member)
    {
    }
  }
}