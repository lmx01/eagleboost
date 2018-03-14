// Author : Shuo Zhang
// 
// Creation :2018-03-13 22:48

namespace eagleboost.core.Contracts.AutoNotify
{
  using System;
  using eagleboost.core.ComponentModel.AutoNotify;

  /// <summary>
  /// IMethodAutoInvoked
  /// </summary>
  public interface IMethodAutoInvoked
  {
    #region Methods
    void OnMethodInvoked(string name, InvokeContext context);
    #endregion Methods

    #region Events
    event MethodInvokedEventHandler MethodInvoked;
    #endregion Events
  }

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
}