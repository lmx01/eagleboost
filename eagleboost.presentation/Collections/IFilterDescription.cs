namespace eagleboost.presentation.Collections
{
  using System.ComponentModel;

  public interface IFilterDescription : INotifyPropertyChanged
  {
    #region Methods
    bool Match(object obj);
    #endregion Methods
  }
}