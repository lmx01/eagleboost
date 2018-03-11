// Author : Shuo Zhang
// 
// Creation :2018-03-09 21:46

namespace eagleboost.core.ComponentModel.AutoNotify
{
  using System.Collections.Generic;
  using System.Reflection;
  using eagleboost.core.Contracts.AutoNotify;

  public class AutoNotifyConfig<T> : IAutoNotifyConfig
  {
    #region ctors
    public static readonly AutoNotifyConfig<T> Instance;

    static AutoNotifyConfig()
    {
      Instance = new AutoNotifyConfig<T>();
    }
    #endregion ctors

    #region IAutoNotifyConfig
    public Dictionary<string, List<string>> NotifyMap { get; private set; }

    public Dictionary<string, List<string>> InvalidateMap { get; private set; }

    public Dictionary<string, List<MethodInfo>> InvokeMap { get; private set; }
    #endregion IAutoNotifyConfig

    #region Public Methods
    public void SetNotifyMap(Dictionary<string, List<string>> notifyMap)
    {
      if (NotifyMap == null)
      {
        NotifyMap = notifyMap;
      }
    }

    public void SetInvalidateMap(Dictionary<string, List<string>> invalidateMap)
    {
      if (InvalidateMap == null)
      {
        InvalidateMap = invalidateMap;
      }
    }

    public void SetInvokeMap(Dictionary<string, List<MethodInfo>> invokeMap)
    {
      if (InvokeMap == null)
      {
        InvokeMap = invokeMap;
      }
    }
    #endregion Public Methods
  }
}