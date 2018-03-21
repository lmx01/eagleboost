// Author : Shuo Zhang
// 
// Creation :2018-03-16 14:35

namespace eagleboost.core.Contracts.AutoComposite
{
  public interface IAutoComposite<out T>
  {
    #region Properties
    T CompositeSource { get; }
    #endregion Properties
  }
}