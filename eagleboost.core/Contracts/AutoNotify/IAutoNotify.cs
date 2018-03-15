// Author : Shuo Zhang
// 
// Creation :2018-03-08 17:22

namespace eagleboost.core.Contracts.AutoNotify
{
  using System.ComponentModel;

  public interface IAutoNotify : INotifyPropertyChanging, INotifyPropertyChanged
  {
    #region Properties
    IAutoNotifyConfig Config { get; }
    #endregion Properties
  }
}