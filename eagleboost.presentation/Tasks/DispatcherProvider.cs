namespace eagleboost.presentation.Tasks
{
  using System.Threading.Tasks;
  using System.Windows.Threading;
  using eagleboost.presentation.Contracts;

  public class DispatcherProvider : IDispatcherProvider
  {
    #region ctors
    public DispatcherProvider()
    {
      Dispatcher = Dispatcher.CurrentDispatcher;
      TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
    }
    #endregion ctors

    #region IDispatcherProvider
    public Dispatcher Dispatcher { get; private set; }

    public TaskScheduler TaskScheduler { get; private set; }
    #endregion IDispatcherProvider
  }
}