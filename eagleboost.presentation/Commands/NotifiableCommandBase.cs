using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading;
using System.Windows.Input;

namespace eagleboost.presentation.Commands
{
  using System.Windows.Threading;
  using eagleboost.presentation.Extensions;

  public abstract class NotifiableCommandBase : ICommand, IDisposable
  {
    private IDisposable _disposable;
    private readonly string[] _properties;
    private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

    protected NotifiableCommandBase(INotifyPropertyChanged notifier, params string[] properties)
    {
      _properties = properties;
      notifier.PropertyChanged += HandleNotifierPropertyChanged;
      _disposable = Disposable.Create(() => notifier.PropertyChanged -= HandleNotifierPropertyChanged);
    }

    private void HandleNotifierPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (_properties == null || _properties.Length == 0 || _properties.Contains(e.PropertyName))
      {
        _dispatcher.CheckedInvoke(RaiseCanExecuteChanged);
      }
    }

    public abstract bool CanExecute(object parameter);

    public abstract void Execute(object parameter);

    public event EventHandler CanExecuteChanged;

    protected virtual void RaiseCanExecuteChanged()
    {
      var handler = CanExecuteChanged;
      if (handler != null)
      {
        handler(this, EventArgs.Empty);
      }
    }

    public void Dispose()
    {
      var disposable = Interlocked.Exchange(ref _disposable, null);
      if (disposable != null)
      {
        disposable.Dispose();
      }
    }
  }
}