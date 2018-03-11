using System.ComponentModel;
using System.Linq.Expressions;

namespace eagleboost.presentation.Collections
{
  public interface IFilterDescription : INotifyPropertyChanged
  {
    #region Methods
    bool Match(object obj);
    #endregion Methods
  }
}