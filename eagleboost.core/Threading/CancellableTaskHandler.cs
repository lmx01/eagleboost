// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 15 5:54 PM

namespace eagleboost.core.Threading
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;

  /// <summary>
  /// CancellableTaskHandler
  /// </summary>
  public sealed class CancellableTaskHandler : IDisposable
  {
    #region Declarations
    private CancellationTokenSource _cts;
    #endregion Declarations

    #region IDisposable
    void IDisposable.Dispose()
    {
      CancelTask();
    }
    #endregion IDisposable

    #region Public Methods
    public void Cancel()
    {
      CancelTask();
    }

    public Task ExecuteAsync(Func<CancellationToken, Task> taskFunc, double timeout)
    {
      CancelTask();

      _cts = new CancellationTokenSource();
      return _cts.Token.TimeoutAsync(taskFunc, timeout);
    }

    public Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> taskFunc, double timeout)
    {
      CancelTask();

      _cts = new CancellationTokenSource();
      return _cts.Token.TimeoutAsync(taskFunc, timeout);
    }
    #endregion Public Methods

    #region Private Methods
    private void CancelTask()
    {
      if (_cts != null)
      {
        _cts.Cancel();
        _cts = null;
      }
    }
    #endregion Private Methods
  }
}