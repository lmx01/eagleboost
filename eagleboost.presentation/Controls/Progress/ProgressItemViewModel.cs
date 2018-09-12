// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-04 11:32 PM

namespace eagleboost.presentation.Controls.Progress
{
  using System;
  using System.ComponentModel;
  using System.Threading;
  using System.Windows.Input;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.Threading;
  using eagleboost.presentation.Contracts;
  using Prism.Commands;
  using Unity.Attributes;

  /// <summary>
  /// IProgressItemViewModel
  /// </summary>
  public interface IProgressItemViewModel : INotifyPropertyChanged, IDisposable, IHeader
  {
    #region Properties
    string Description { get; }

    TimeSpan? TimeElapsed { get; }

    TimeSpan? TimeRemaining { get; }

    int? AverageSpeed { get; }

    bool HasRemaining { get; }

    int Current { get; }

    int? Total { get; }

    double Progress { get; }

    object State { get; }

    ICommand PauseCommand { get; }

    ICommand ResumeCommand { get; }

    ICommand CancelCommand { get; }
    #endregion Properties

    #region Events
    event EventHandler Completed;

    event EventHandler Canceled;
    #endregion Events
  }

  /// <summary>
  /// IProgressItemViewModel
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IProgressItemViewModel<T> : IProgressItemViewModel where T : class
  {
    #region Properties
    new T State { get; }
    #endregion Properties
  }

  /// <summary>
  /// ProgressItemViewModel
  /// </summary>
  public abstract class ProgressItemViewModel<T> : NotifyPropertyChangedBase, IProgressItemViewModel<T>, IProgress<T> where T : class
  {
    #region Statics
    protected static readonly PropertyChangedEventArgs ProgressArgs = GetChangedArgs<ProgressItemViewModel<T>>(o => o.Progress);
    protected static readonly PropertyChangedEventArgs DescriptionArgs = GetChangedArgs<ProgressItemViewModel<T>>(o => o.Description);
    protected static readonly PropertyChangedEventArgs TimeElapsedArgs = GetChangedArgs<ProgressItemViewModel<T>>(o => o.TimeElapsed);
    protected static readonly PropertyChangedEventArgs HasRemainingArgs = GetChangedArgs<ProgressItemViewModel<T>>(o => o.HasRemaining);
    protected static readonly PropertyChangedEventArgs AverageSpeedArgs = GetChangedArgs<ProgressItemViewModel<T>>(o => o.AverageSpeed);
    #endregion Statics

    #region Declarations
    private string _header;
    private string _desc;
    private double _progress;
    private int _current;
    private int? _total;
    private TimeSpan? _remaining;
    private DateTime? _startTime;
    private Timer _timer;
    private T _state;
    #endregion Declarations

    #region ctors
    protected ProgressItemViewModel(PauseTokenSource pts, CancellationTokenSource cts)
    {
      PauseCommand = new DelegateCommand(() => pts.IsPaused = true, () => !pts.IsPaused);
      ResumeCommand = new DelegateCommand(() => pts.IsPaused = false, () => pts.IsPaused);
      CancelCommand = new DelegateCommand(() => HandleCancel(cts));
    }
    #endregion ctors

    #region IProgressItemViewModel
    public string Header
    {
      get { return _header; }
      set { SetValue(ref _header, value); }
    }

    public string Description
    {
      get { return _desc; }
      set { SetValue(ref _desc, value); }
    }

    public TimeSpan? TimeElapsed
    {
      get
      {
        if (_startTime.HasValue)
        {
          return DateTime.Now - _startTime.Value;
        }

        return null;
      }
    }

    public int? AverageSpeed
    {
      get
      {
        if (Current > 0)
        {
          return (int?) Math.Floor(Current / TimeElapsed.Value.TotalMinutes);
        }

        return null;
      }
    }

    public bool HasRemaining
    {
      get { return TimeRemaining.HasValue; }
    }

    public int Current
    {
      get { return _current; }
      set { SetValue(ref _current, value); }
    }

    public int? Total
    {
      get { return _total; }
      set { SetValue(ref _total, value); }
    }

    public TimeSpan? TimeRemaining
    {
      get { return _remaining; }
      set { SetValue(ref _remaining, value); }
    }

    public double Progress
    {
      get { return _progress; }
      set { SetValue(ref _progress, value); }
    }

    object IProgressItemViewModel.State
    {
      get { return State; }
    }

    public ICommand PauseCommand { get; private set; }

    public ICommand ResumeCommand { get; private set; }

    public ICommand CancelCommand { get; private set; }

    public event EventHandler Completed;

    protected virtual void RaiseCompleted()
    {
      var handler = Completed;
      if (handler != null)
      {
        handler(this, EventArgs.Empty);
      }
    }

    public event EventHandler Canceled;

    protected virtual void RaiseCanceled()
    {
      var handler = Canceled;
      if (handler != null)
      {
        handler(this, EventArgs.Empty);
      }
    }

    public T State
    {
      get { return _state; }
      private set { Force(ref _state, value); }
    }
    #endregion IProgressItemViewModel

    #region IProgress
    public void Report(T value)
    {
      State = value;
      DoReport(value);
    }
    #endregion IProgress

    #region IDisposable
    public void Dispose()
    {
      if (_timer != null)
      {
        _timer.Dispose();
      }
    }
    #endregion IDisposable

    #region Init
    [InjectionMethod]
    public void Initialize()
    {
      OnInitialize();

      _startTime = DateTime.Now;
      _timer = new Timer(HandleTimerTick, null, 0, 1000);
    }
    #endregion Init

    #region Virtuals
    protected abstract void DoReport(T value);

    protected virtual void OnInitialize()
    {
    }
    #endregion Virtuals

    #region Public Methods
    public void Complete()
    {
      RaiseCompleted();
    }
    #endregion Public Methods

    #region Event Handlers
    private void HandleTimerTick(object obj)
    {
      NotifyPropertyChanged(TimeElapsedArgs);
    }
    #endregion Event Handlers

    #region Private Methods
    public void HandleCancel(CancellationTokenSource cts)
    {
      cts.Cancel();
      RaiseCanceled();
    }
    #endregion Private Methods
  }
}