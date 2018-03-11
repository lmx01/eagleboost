namespace eagleboost.core.Collections
{
  using System;
  using System.Collections.ObjectModel;
  using System.Collections.Specialized;
  using System.ComponentModel;
  using System.Reactive.Disposables;

  public class BatchObservableCollection<T> : ObservableCollection<T>
  {
    #region Statics
    private static readonly PropertyChangedEventArgs ItemArgs = new PropertyChangedEventArgs("Item[]");
    private static readonly PropertyChangedEventArgs CountArgs = new PropertyChangedEventArgs("Count");
    #endregion Statics

    #region Declarations
    private bool _isNotificationSuspended;
    #endregion Declarations

    #region public Methods
    public IDisposable SuspendNotification()
    {
      if (_isNotificationSuspended)
      {
        throw new InvalidOperationException("Batch update already in progress");
      }

      _isNotificationSuspended = true;
      return Disposable.Create(ResumeNotification);
    }
    
    public IDisposable SuspendNotification(Action<BatchObservableCollection<T>> action)
    {
      if (_isNotificationSuspended)
      {
        throw new InvalidOperationException("Batch update already in progress");
      }

      _isNotificationSuspended = true;
      action(this);
      return Disposable.Create(ResumeNotification);
    }
    #endregion public Methods

    #region Overrides
    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      if (!_isNotificationSuspended)
      {
        base.OnCollectionChanged(e);
      }
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      if (!_isNotificationSuspended)
      {
        base.OnPropertyChanged(e);
      }
    }
    #endregion Overrides

    #region Private Methods
    private void ResumeNotification()
    {
      if (_isNotificationSuspended)
      {
        _isNotificationSuspended = false;
        OnPropertyChanged(CountArgs);
        OnPropertyChanged(ItemArgs);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
      }
    }
    #endregion Private Methods
  }
}
