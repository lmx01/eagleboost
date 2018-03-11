// Author : Shuo Zhang
// 
// Creation :2018-03-09 16:09

namespace eagleboost.core.ComponentModel.AutoNotify
{
  using System.Reflection;

  public class InvokeBy<T> : AutoNotifyOperation<InvokeBy<T>, T, MethodInfo>
  {
    public InvokeBy(MethodInfo toNotify) : base(toNotify)
    {
    }
  }
}