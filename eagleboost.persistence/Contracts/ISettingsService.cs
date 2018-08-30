// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-28 1:28 AM

namespace eagleboost.persistence.Contracts
{
  public interface ISettingsService
  {
    #region Methods
    void Save<T>(string key, T setting) where T : class;

    T Load<T>(string key) where T : class;
    #endregion Methods
  }
}