namespace eagleboost.core.Data
{
  using System;
  using System.Collections.Generic;
  using System.Reactive.Disposables;
  using System.Threading;

  public class DisposeManager : IDisposable
  {
    #region Declarations
    private List<IDisposable> _disposables =new List<IDisposable>();
    #endregion Declarations

    public void Dispose()
    {
      var disposables = Interlocked.Exchange(ref _disposables, null);
      if (disposables != null)
      {
        disposables.ForEach(d => d.Dispose());
      }
    }

    public void AddEvent<T>(Action<T> add, Action<T> remove, T handler)
    {
      add(handler);
      var d = Disposable.Create(() => remove(handler));
      _disposables.Add(d);
    }
  }
}