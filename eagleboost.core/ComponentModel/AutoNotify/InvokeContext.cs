// Author : Shuo Zhang
// 
// Creation :2018-03-09 21:37

namespace eagleboost.core.ComponentModel.AutoNotify
{
  using System.Diagnostics;

  [DebuggerDisplay("{Property} : {_oldValue}=>{_newValue}")]
  public class InvokeContext
  {
    #region Declarations
    private readonly object _oldValue;
    private readonly object _newValue;
    #endregion Declarations

    #region Statics
    public static InvokeContext Create(string property, object oldValue, object newValue)
    {
      return new InvokeContext(property, oldValue, newValue);
    }
    #endregion Statics

    #region ctors
    private InvokeContext(string property, object oldValue, object newValue)
    {
      Property = property;
      _oldValue = oldValue;
      _newValue = newValue;
    }
    #endregion ctors

    #region Public Members
    public string Property { get; private set; }

    public T OldValue<T>()
    {
      return (T) _oldValue;
    }

    public T NewValue<T>()
    {
      return (T)_newValue;
    }
    #endregion Public Members
  }
}