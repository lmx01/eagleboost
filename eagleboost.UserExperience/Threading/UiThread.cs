// Author : Shuo Zhang
// 
// Creation :2018-03-20 20:51

namespace eagleboost.UserExperience.Threading
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using System.Windows.Threading;

  public class UiThread
  {
    #region Declarations
    private static readonly List<UiThread> _allThreads = new List<UiThread>();
    #endregion Declarations

    #region ctors
    [ThreadStatic]
    public static UiThread Current;

    public UiThread()
    {
      Thread = Thread.CurrentThread;
      Debug.Assert(Thread.GetApartmentState() == ApartmentState.STA);
      Dispatcher = Dispatcher.CurrentDispatcher;
      TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
    }
    #endregion ctors

    #region Public Properties
    public readonly Thread Thread;

    public readonly Dispatcher Dispatcher;

    public readonly TaskScheduler TaskScheduler;

    public static IReadOnlyCollection<UiThread> AllThreads
    {
      get { return _allThreads; }
    }
    #endregion Public Properties

    #region Public Methods

    public static void Initialize()
    {
      Current = new UiThread();
      _allThreads.Add(Current);
    }

    public static Dispatcher GetDispatcher(string threadName)
    {
      var thread = AllThreads.FirstOrDefault(t => t.Thread.Name == threadName);
      return thread != null ? thread.Dispatcher : null;
    }
    #endregion Public Methods
  }
}