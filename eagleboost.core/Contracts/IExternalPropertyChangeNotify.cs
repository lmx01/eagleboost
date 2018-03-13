// Author : Shuo Zhang
// 
// Creation :2018-03-04 08:42

namespace eagleboost.core.Contracts
{
  public interface IExternalPropertyChangeNotify
  {
    #region Methods
    void OnPropertyChanging(string propertyName);

    void OnPropertyChanged(string propertyName);
    #endregion Methods
  }
}