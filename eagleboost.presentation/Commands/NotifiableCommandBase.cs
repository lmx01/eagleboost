using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading;
using System.Windows.Input;

namespace eagleboost.presentation.Commands
{
  public abstract class NotifiableCommandBase : ICommand, IDisposable
  {
    private IDisposable _disposable;
    private readonly string[] _properties;

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
        RaiseCanExecuteChanged();
      }
    }

    public abstract bool CanExecute(object parameter);

    public abstract void Execute(object parameter);

    public event EventHandler CanExecuteChanged;

    protected virtual void RaiseCanExecuteChanged()
    {
      CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Dispose()
    {
      var disposable = Interlocked.Exchange(ref _disposable, null);
      disposable?.Dispose();
    }
  }
}