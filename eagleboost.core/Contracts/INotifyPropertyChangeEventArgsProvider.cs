// Author : Shuo Zhang
// 
// Creation :2018-03-04 10:19

namespace eagleboost.core.Contracts
{
  using System.ComponentModel;

  public interface INotifyPropertyChangeEventArgsProvider
  {
    #region Methods
    PropertyChangedEventArgs GetPropertyChangedArgs(string propertyName);

    PropertyChangingEventArgs GetPropertyChangingArgs(string propertyName);
    #endregion Methods
  }
}