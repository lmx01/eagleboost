// Author : Shuo Zhang
// 
// Creation :2018-03-05 22:58

namespace eagleboost.core.Commands
{
  using System;
  using System.Linq.Expressions;
  using eagleboost.core.Contracts;

  public class NotifiableCommand : IValidatableCommand
  {
    #region Declarations
    private readonly Action _execute;
    private readonly Func<bool> _canExecute;
    #endregion Declarations

    #region ctors
    public NotifiableCommand(Action execute, Expression<Func<bool>> canExecute)
    {
      _execute = execute;
      _canExecute = canExecute.Compile();
    }
    #endregion ctors

    public bool CanExecute(object parameter)
    {
      return _canExecute();
    }

    public void Execute(object parameter)
    {
      _execute();
    }

    public event EventHandler CanExecuteChanged;

    void IValidatableCommand.Invalidate()
    {
      CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}