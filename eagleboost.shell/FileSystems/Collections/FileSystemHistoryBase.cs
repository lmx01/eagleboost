// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 23 8:42 PM

namespace eagleboost.shell.FileSystems.Collections
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.Extensions;
  using eagleboost.shell.FileSystems.Contracts;

  /// <summary>
  /// FileSystemHistoryBase
  /// </summary>
  public class FileSystemHistoryBase : NotifyPropertyChangedBase
  {
    #region Declarations
    private ObservableCollection<IFileSystemHistoryOperations> _historyEntries;
    private readonly Stack<IFileSystemHistoryOperations> _historyStack = new Stack<IFileSystemHistoryOperations>();
    #endregion Declarations

    #region ctors
    public FileSystemHistoryBase()
    {
      UpdateHistoryEntries();
    }
    #endregion ctors

    #region Public Properties
    public ObservableCollection<IFileSystemHistoryOperations> HistoryEntries
    {
      get { return _historyEntries; }
      private set { SetValue(ref _historyEntries, value); }
    }
    #endregion Public Properties

    #region Events
    public event EventHandler HistoryUpdated;

    protected virtual void RaiseHistoryUpdated()
    {
      var handler = HistoryUpdated;
      if (handler != null)
      {
        handler(this, EventArgs.Empty);
      }
    }
    #endregion Events

    #region Overrides

    protected override void OnPropertyChanged(string propertyName)
    {
      base.OnPropertyChanged(propertyName);

      if (propertyName == this.Property(o => o.HistoryEntries))
      {
        RaiseHistoryUpdated();
      }
    }
    #endregion Overrides

    #region Public Methods
    public void Push(IFileSystemHistoryOperations entry)
    {
      _historyStack.Push(entry);
      UpdateHistoryEntries();
    }

    public IFileSystemHistoryOperations Pop()
    {
      var result = _historyStack.Pop();
      UpdateHistoryEntries();
      return result;
    }

    public void Clear()
    {
      _historyStack.Clear();
      UpdateHistoryEntries();
    }
    #endregion Public Methods

    #region Private Methods
    private void UpdateHistoryEntries()
    {
      HistoryEntries = new ObservableCollection<IFileSystemHistoryOperations>(_historyStack);
    }
    #endregion Private Methods
  }
}