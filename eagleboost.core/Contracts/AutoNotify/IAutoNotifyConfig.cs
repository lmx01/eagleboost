// Author : Shuo Zhang
// 
// Creation :2018-02-23 23:02

namespace eagleboost.core.Contracts.AutoNotify
{
  using System.Collections.Generic;
  using System.Reflection;

  public interface IAutoNotifyConfig
  {
    Dictionary<string, List<string>> NotifyMap { get; }
    Dictionary<string, List<string>> InvalidateMap { get; }
    Dictionary<string, List<MethodInfo>> InvokeMap { get; }
  }
}