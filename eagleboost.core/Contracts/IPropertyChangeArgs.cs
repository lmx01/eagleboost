// Author : Shuo Zhang
// 
// Creation :2018-03-12 21:58

namespace eagleboost.core.Contracts
{
  using System.ComponentModel;

  public interface IPropertyChangeArgs
  {
    #region Methods
    PropertyChangedEventArgs GetChangedArgs(string propertyName);

    PropertyChangingEventArgs GetChangingArgs(string propertyName);
    #endregion Methods
  }
}