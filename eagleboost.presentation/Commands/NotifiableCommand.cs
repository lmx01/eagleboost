namespace eagleboost.presentation.Commands
{
  using System;
  using System.ComponentModel;

  public class NotifiableCommand<T> : NotifiableCommandBase
  {
    private readonly Action<T> _executeAction;
    private readonly Func<T, bool> _canExecuteFunc;

    public NotifiableCommand(Action<T> executeAction, Func<T,bool> canExecuteFunc, INotifyPropertyChanged notifier, params string[] properties)
      : base(notifier, properties)
    {
      _executeAction = executeAction;
      _canExecuteFunc = canExecuteFunc;
    }

    public NotifiableCommand(Action<T> executeAction, INotifyPropertyChanged notifier, params string[] properties)
      : this(executeAction, e => true, notifier, properties)
    {
    }

    public override bool CanExecute(object parameter)
    {
      return _canExecuteFunc((T)parameter);
    }

    public override void Execute(object parameter)
    {
      _executeAction((T)parameter);
    }
  }

  public class NotifiableCommand : NotifiableCommandBase
  {
    private readonly Action _executeAction;
    private readonly Func<bool> _canExecuteFunc;

    public NotifiableCommand(Action executeAction, Func<bool> canExecuteFunc, INotifyPropertyChanged notifier, params string[] properties)
      : base(notifier, properties)
    {
      _executeAction = executeAction;
      _canExecuteFunc = canExecuteFunc;
    }

    public NotifiableCommand(Action executeAction, INotifyPropertyChanged notifier, params string[] properties)
      : this(executeAction, () => true, notifier, properties)
    {
    }

    public override bool CanExecute(object parameter)
    {
      return _canExecuteFunc();
    }

    public override void Execute(object parameter)
    {
      _executeAction();
    }
  }
}