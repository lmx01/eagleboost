// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-04 11:32 PM

namespace eagleboost.presentation.Controls.Progress
{
  using System;
  using System.ComponentModel;
  using System.Windows.Input;
  using System.Windows.Threading;
  using eagleboost.core.Commands;
  using eagleboost.core.Threading;
  using eagleboost.presentation.Contracts;
  using eagleboost.presentation.Interactivity;
  using Unity.Attributes;

  /// <summary>
  /// IProgressViewModel
  /// </summary>
  public interface IProgressViewModel<T> : IViewController, IDisposable, IHeader
  {
    #region Properties
    string Description { get; }

    TimeSpan? TimeElapsed { get; }

    TimeSpan TimeRemaining { get; }

    double Progress { get; }

    ICommand PauseCommand { get; }

    ICommand ResumeCommand { get; }
    #endregion Properties

    #region Events
    event EventHandler Completed;
    #endregion Events
  }

  /// <summary>
  /// ProgressViewModel
  /// </summary>
  public abstract class ProgressViewModel<T> : ViewController, IProgressViewModel<T>, IProgress<T>
  {
    #region Statics
    protected static readonly PropertyChangedEventArgs ProgressArgs = GetChangedArgs<ProgressViewModel<T>>(o => o.Progress);
    protected static readonly PropertyChangedEventArgs DescriptionArgs = GetChangedArgs<ProgressViewModel<T>>(o => o.Description);
    protected static readonly PropertyChangedEventArgs TimeElapsedArgs = GetChangedArgs<ProgressViewModel<T>>(o => o.TimeElapsed);
    #endregion Statics

    #region Declarations
    private string _desc;
    private double _progress;
    private TimeSpan _remaining;
    private DateTime? _startTime;
    private DispatcherTimer _timer;
    #endregion Declarations

    #region ctors
    protected ProgressViewModel(PauseTokenSource pts)
    {
      PauseCommand = new NotifiableCommand(() => pts.IsPaused = true, () => !pts.IsPaused);
      ResumeCommand = new NotifiableCommand(() => pts.IsPaused = false, () => pts.IsPaused);
    }
    #endregion ctors

    #region IProgressViewModel
    public string Header { get; set; }

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

    public TimeSpan TimeRemaining
    {
      get { return _remaining; }
      set { SetValue(ref _remaining, value); }
    }

    public double Progress
    {
      get { return _progress; }
      set { SetValue(ref _progress, value); }
    }

    public ICommand PauseCommand { get; private set; }

    public ICommand ResumeCommand { get; private set; }

    public event EventHandler Completed;

    protected virtual void RaiseCompleted()
    {
      var handler = Completed;
      if (handler != null)
      {
        handler(this, EventArgs.Empty);
      }
    }
    #endregion IProgressViewModel

    #region IProgress
    public abstract void Report(T value);
    #endregion IProgress

    #region IDisposable
    public void Dispose()
    {
      if (_timer != null)
      {
        _timer.Tick -= HandleTimerTick;
        _timer.Stop();
        _timer = null;
      }
    }
    #endregion IDisposable

    #region Init
    [InjectionMethod]
    public void Initialize()
    {
      _startTime = DateTime.Now;
      _timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};
      _timer.Tick += HandleTimerTick;
      _timer.Start();
    }
    #endregion Init

    #region Public Methods
    public void Complete()
    {
      RaiseCompleted();
    }
    #endregion Public Methods

    #region Event Handlers
    private void HandleTimerTick(object sender, EventArgs e)
    {
      NotifyPropertyChanged(TimeElapsedArgs);
    }
    #endregion Event Handlers
  }
}