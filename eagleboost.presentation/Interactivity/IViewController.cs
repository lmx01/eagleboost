using System;
using System.Windows.Input;

namespace eagleboost.presentation.Interactivity
{
  public interface IViewController
  {
    #region Properties
    ICommand OkCommand { get; }

    ICommand CancelCommand { get; }
    #endregion

    #region Methods
    void AddCloseAction(Action<bool?> closeAction);
    #endregion Methods
  }
}