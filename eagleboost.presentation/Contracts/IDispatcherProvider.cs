namespace eagleboost.presentation.Contracts
{
  using System.Threading.Tasks;
  using System.Windows.Threading;

  public interface IDispatcherProvider
  {
    #region Properties
    Dispatcher Dispatcher { get; }

    TaskScheduler TaskScheduler { get; }
    #endregion Properties
  }
}