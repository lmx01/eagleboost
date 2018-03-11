// Author : Shuo Zhang
// 
// Creation :2018-03-08 17:22

namespace eagleboost.core.Contracts.AutoNotify
{
  using System;
  using eagleboost.core.ComponentModel.AutoNotify;

  public class MethodInvokedEventArgs : EventArgs
  {
    #region ctors
    public MethodInvokedEventArgs(string name, InvokeContext context)
    {
      Name = name;
      InvokeContext = context;
    }
    #endregion ctors

    #region Public Properties
    public string Name { get; private set; }

    public InvokeContext InvokeContext { get; private set; }
    #endregion Public Properties
  }

  public delegate void MethodInvokedEventHandler(object sender, MethodInvokedEventArgs e);

  public interface IAutoNotify
  {
    #region Properties
    IAutoNotifyConfig Config { get; }
    #endregion Properties

    #region Events
    event MethodInvokedEventHandler MethodInvoked;
    #endregion Events
  }

  /// <summary>
  /// IMethodInvoked
  /// </summary>
  public interface IMethodInvoked
  {
    #region Methods
    void OnMethodInvoked(string name, InvokeContext context);
    #endregion Methods
  }
}