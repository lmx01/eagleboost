// Author : Shuo Zhang
// 
// Creation :2018-01-21 09:22

namespace eagleboost.core.Contracts
{
  using System.Windows.Input;

  public interface IInvalidatableCommand : ICommand
  {
    #region Methods
    void Invalidate();
    #endregion Methods 
  }
}