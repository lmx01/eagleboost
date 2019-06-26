// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 25 8:05 PM

namespace eagleboost.shell.FileSystems.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Windows.Data;
  using System.Windows.Input;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.Extensions;
  using eagleboost.presentation.Extensions;
  using eagleboost.shell.FileSystems.Contracts;
  using Prism.Commands;

  /// <summary>
  /// FileSystemHistory
  /// </summary>
  public sealed class FileSystemHistory : NotifyPropertyChangedBase
  {
    #region Declarations
    private ICollectionView _backwardHistoryView;
    private ICollectionView _forwardHistoryView;
    private IFileSystemHistoryOperations _current;
    private readonly DelegateCommand _backCommand;
    private readonly DelegateCommand _forwardCommand;
    private readonly ObservableCollection<IFileSystemHistoryOperations> _historyEntries;
    #endregion Declarations

    #region ctors
    public FileSystemHistory()
    {
      _historyEntries = new ObservableCollection<IFileSystemHistoryOperations>();
      _backCommand = new DelegateCommand(NavigateBack, () => HasBackwardHistory);
      _forwardCommand = new DelegateCommand(NavigateForward, () => HasForwardHistory);
    }
    #endregion ctors

    #region Public Properties
    public IReadOnlyCollection<IFileSystemHistoryOperations> HistoryEntries
    {
      get { return _historyEntries; }
    }

    public ICollectionView BackwardHistoryView
    {
      get { return _backwardHistoryView ?? (_backwardHistoryView = CreateBackwardHistoryView()); }
    }

    public bool HasBackwardHistory
    {
      get { return !BackwardHistoryView.IsEmpty; }
    }

    public ICollectionView ForwardHistoryView
    {
      get { return _forwardHistoryView ?? (_forwardHistoryView = CreateForwardHistoryView()); }
    }

    public bool HasForwardHistory
    {
      get { return !ForwardHistoryView.IsEmpty; }
    }

    public IFileSystemHistoryOperations Current
    {
      get { return _current; }
      private set { SetValue(ref _current, value); }
    }

    public ICommand BackCommand
    {
      get { return _backCommand; }
    }

    public ICommand ForwardCommand
    {
      get { return _forwardCommand; }
    }
    #endregion Public Properties

    #region Public Methods
    public void Add(IFileSystemHistoryOperations entry)
    {
      var c = Current;
      if (c != null)
      {
        var index = _historyEntries.IndexOf(Current);
        for (var i = _historyEntries.Count-1; i > index; i--)
        {
          var forwardItem = _historyEntries[i];
          _historyEntries.Remove(forwardItem);
        }
      }

      entry.Navigate -= HandleEntryNavigate;
      entry.Navigate += HandleEntryNavigate;
      _historyEntries.Add(entry);
      Current = entry;
    }

    public void NavigateBack()
    {
      if (HasBackwardHistory)
      {
        var index = _historyEntries.IndexOf(Current);
        var prev = _historyEntries[index - 1];
        prev.NavigateTo();
        Current = prev;
      }
    }

    public void NavigateForward()
    {
      if (HasForwardHistory)
      {
        var index = _historyEntries.IndexOf(Current);
        var next = _historyEntries[index + 1];
        next.NavigateTo();
        Current = next;
      }
    }

    public void Clear()
    {
      _historyEntries.Clear();
      Current = null;
    }

    public IFileSystemHistoryOperations GetNext(IFileSystemHistoryOperations entry)
    {
      var index = _historyEntries.IndexOf(entry);
      if (index == _historyEntries.Count - 1)
      {
        return null;
      }

      return _historyEntries[index + 1];
    }
    #endregion Public Methods

    #region Overrides
    protected override void OnPropertyChanged(string propertyName)
    {
      base.OnPropertyChanged(propertyName);

      if (propertyName == this.Property(o => o.Current))
      {
        RefreshBackwardHistory();
        RefreshForwardHistory();
        _backCommand.RaiseCanExecuteChanged();
        _forwardCommand.RaiseCanExecuteChanged();
      }
    }
    #endregion Overrides

    #region Event Handlers
    private void HandleEntryNavigate(object sender, EventArgs e)
    {
      var entry = (IFileSystemHistoryOperations)sender;
      Current = entry;
    }
    #endregion Event Handlers

    #region Private Methods
    private void RefreshBackwardHistory()
    {
      BackwardHistoryView.Refresh();
      NotifyPropertyChanged(this.Property(o => o.HasBackwardHistory));
    }

    private void RefreshForwardHistory()
    {
      ForwardHistoryView.Refresh();
      NotifyPropertyChanged(this.Property(o => o.HasForwardHistory));
    }

    private ICollectionView CreateHistoryView(Func<IFileSystemHistoryOperations, bool> filter)
    {
      var view = new ListCollectionView(_historyEntries)
      {
        Filter = o => filter((IFileSystemHistoryOperations)o)
      };
      return view;
    }

    private ICollectionView CreateBackwardHistoryView()
    {
      return CreateHistoryView(IsBackwardEntry);
    }

    private bool IsBackwardEntry(IFileSystemHistoryOperations entry)
    {
      var c = Current;
      if (entry == c)
      {
        return false;
      }

      var currentIndex = _historyEntries.IndexOf(c);
      var index = _historyEntries.IndexOf(entry);
      return index < currentIndex;
    }

    private ICollectionView CreateForwardHistoryView()
    {
      return CreateHistoryView(IsForwardEntry);
    }

    private bool IsForwardEntry(IFileSystemHistoryOperations entry)
    {
      var c = Current;
      if (entry == c)
      {
        return false;
      }

      var currentIndex = _historyEntries.IndexOf(c);
      var index = _historyEntries.IndexOf(entry);
      return index > currentIndex;
    }
    #endregion Private Methods
  }
}