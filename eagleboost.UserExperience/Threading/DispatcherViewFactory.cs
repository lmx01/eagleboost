// Author : Shuo Zhang
// 
// Creation :2018-03-20 23:00

namespace eagleboost.UserExperience.Threading
{
  using System;
  using System.AddIn.Contract;
  using System.AddIn.Pipeline;
  using System.Threading;
  using System.Threading.Tasks;
  using System.Windows;
  using System.Windows.Threading;

  public static class DispatcherViewFactory
  {
    internal static async Task<Dispatcher> GetOrCreateDispatcherAsync(string threadName)
    {
      var dispatcher = UiThread.GetDispatcher(threadName);
      if (dispatcher != null)
      {
        return dispatcher;
      }

      await Task.Run(() =>
      {
        var wait = new ManualResetEventSlim();
        ThreadStart threadStart = () =>
        {
          dispatcher = Dispatcher.CurrentDispatcher;
          dispatcher.BeginInvoke(() =>
          {
            UiThread.Initialize();
            wait.Set();
          });

          Dispatcher.Run();
        };

        var thread = new Thread(threadStart) { Name = threadName, IsBackground = true };
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();

        wait.Wait();
      });

      return dispatcher;
    }

    public static async Task<INativeHandleContract> CreateViewContract(string threadName, Func<FrameworkElement> factory)
    {
      INativeHandleContract contract = null;
      var dispatcher =  await GetOrCreateDispatcherAsync(threadName);
      await dispatcher.BeginInvoke(() => contract = FrameworkElementAdapters.ViewToContractAdapter(factory()));
      return contract;
    }

    public static async Task InvokeAsync(string threadName, Action action)
    {
      var dispatcher = await GetOrCreateDispatcherAsync(threadName);
      await dispatcher.BeginInvoke(() => action());
    }
  }
}