// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-06 4:29 PM

namespace eagleboost.core.Commands
{
  using System;
  using System.Linq.Expressions;
  using eagleboost.core.Contracts;

  public class NotifiableCommand<T> : IInvalidatableCommand
  {
    #region Declarations
    private readonly Action<T> _execute;
    private readonly Func<T, bool> _canExecute;
    #endregion Declarations

    #region ctors
    public NotifiableCommand(Action<T> execute, Expression<Func<T, bool>> canExecute)
    {
      _execute = execute;
      _canExecute = canExecute.Compile();
    }
    #endregion ctors

    public bool CanExecute(object parameter)
    {
      return _canExecute((T)parameter);
    }

    public void Execute(object parameter)
    {
      _execute((T)parameter);
    }

    public event EventHandler CanExecuteChanged;

    void IInvalidatableCommand.Invalidate()
    {
      var handler = CanExecuteChanged;
      if (handler != null)
      {
        handler(this, EventArgs.Empty);
      }
    }
  }
}