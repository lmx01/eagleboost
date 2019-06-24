// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 23 8:33 PM

namespace eagleboost.shell.FileSystems.ViewModels
{
  using System.ComponentModel;
  using System.Linq;
  using System.Runtime.InteropServices.WindowsRuntime;
  using System.Threading.Tasks;
  using System.Windows.Input;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.Extensions;
  using eagleboost.presentation.Collections;
  using eagleboost.presentation.Controls.TreeView;
  using eagleboost.presentation.Extensions;
  using eagleboost.shell.FileSystems.Collections;
  using eagleboost.shell.FileSystems.Contracts;
  using eagleboost.shell.FileSystems.Extensions;
  using Prism.Commands;

  /// <summary>
  /// FileSystemHistoryViewModel
  /// </summary>
  /// <typeparam name="TFile"></typeparam>
  /// <typeparam name="TFolder"></typeparam>
  public class FileSystemHistoryViewModel<TFile, TFolder> : NotifyPropertyChangedBase, IFileSystemHistoryViewModel<TFile, TFolder>
    where TFile : class, IFile
    where TFolder : class, IFolder
  {
    #region Declarations
    private readonly FileSystemBackwardHistory _backwardHistory;
    private readonly FileSystemForwardHistory _forwardHistory;
    private IFileSystemTreeViewModel _treeViewModel;
    private IFileSystemCollectionViewModel<TFile, TFolder> _gridViewModel;
    private DelegateCommand _backCommand;
    private DelegateCommand _forwardCommand;
    private IFileSystemHistoryOperations _current;
    #endregion Declarations

    #region ctors
    public FileSystemHistoryViewModel()
    {
      _backwardHistory = new FileSystemBackwardHistory();
      _backwardHistory.HistoryUpdated += HandleBackwardHistoryUpdated;
      _forwardHistory = new FileSystemForwardHistory();
      _forwardHistory.HistoryUpdated += HandleForwardHistoryUpdated;
    }
    #endregion ctors

    #region IFileSystemHistoryViewModel
    public FileSystemBackwardHistory BackwardHistory
    {
      get { return _backwardHistory; }
    }

    public FileSystemForwardHistory ForwardHistory
    {
      get { return _forwardHistory; }
    }

    public IFileSystemHistoryOperations Current
    {
      get { return _current; }
      private set { SetValue(ref _current, value); }
    }

    public ICommand BackCommand
    {
      get { return _backCommand ?? (_backCommand = new DelegateCommand(HandleNavigateBack, CanNavigateBack)); }
    }

    public ICommand ForwardCommand
    {
      get { return _forwardCommand ?? (_forwardCommand = new DelegateCommand(HandleNavigateForward, CanNavigateForward)); }
    }

    public void Initialize(IFileSystemTreeViewModel treeViewModel, IFileSystemCollectionViewModel<TFile, TFolder> gridViewModel)
    {
      _treeViewModel = treeViewModel;
      _treeViewModel.PropertyChanged += HandleTreePropertyChanged;
      _gridViewModel = gridViewModel;
    }
    #endregion IFileSystemHistoryViewModel

    #region Event Handlers
    private void HandleTreePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.Match<ICollectionViewModel>(v => v.SelectedItem))
      {
        var selected = (TreeNodeContainer)_treeViewModel.SelectedItem;
        if (selected != null)
        {
          UpdateSelectedFolders((TFolder)selected.DataItem);
        }
        else
        {
          UpdateEmptySelectedFolders();
        }
      }
    }

    private void HandleBackwardHistoryUpdated(object sender, System.EventArgs e)
    {
      BackCommand.CastTo<DelegateCommand>().RaiseCanExecuteChanged();
    }

    private void HandleForwardHistoryUpdated(object sender, System.EventArgs e)
    {
      ForwardCommand.CastTo<DelegateCommand>().RaiseCanExecuteChanged();
    }
    #endregion Event Handlers

    #region Private Methods
    private void UpdateSelectedFolders(TFolder folder)
    {
      if (Current == null)
      {
        Current = new FileSystemHistoryOperations(folder, e => { });
      }
      else
      {
        _backwardHistory.Push(new FileSystemHistoryOperations(Current.DriveFolder, HandleNavigateBackward));
        _forwardHistory.Clear();
        Current = new FileSystemHistoryOperations(folder, e => { });
      }
    }

    private void UpdateEmptySelectedFolders()
    {
      _backwardHistory.Clear();
      _forwardHistory.Clear();
    }

    private void HandleNavigateBackward(IFileSystemHistoryOperations backOperation)
    {
      NavigateAsync(backOperation.DriveFolder).ContinueWith(t =>
      {
        _forwardHistory.Push(new FileSystemHistoryOperations(Current.DriveFolder, HandleNavigateForward));
        _backwardHistory.Pop();
        Current = backOperation;
      });
    }

    private void HandleNavigateForward(IFileSystemHistoryOperations forwardOperation)
    {
      NavigateAsync(forwardOperation.DriveFolder).ContinueWith(t =>
      {
        _backwardHistory.Push(new FileSystemHistoryOperations(Current.DriveFolder, HandleNavigateBackward));
        _forwardHistory.Pop();
        Current = forwardOperation;
      });
    }

    private async Task NavigateAsync(IFolder folder)
    {
      _treeViewModel.PropertyChanged -= HandleTreePropertyChanged;
      await _treeViewModel.SelectAsync(folder);
      _treeViewModel.PropertyChanged += HandleTreePropertyChanged;
    }

    private void HandleNavigateBack()
    {
      if (CanNavigateBack())
      {
        var first = _backwardHistory.HistoryEntries.FirstOrDefault();
        if (first != null)
        {
          first.NavigateToCommand.TryExecute(first);
        }
      }
    }

    private bool CanNavigateBack()
    {
      return _backwardHistory.HistoryEntries.Any();
    }

    private void HandleNavigateForward()
    {
      if (CanNavigateForward())
      {
        var first = _forwardHistory.HistoryEntries.FirstOrDefault();
        if (first != null)
        {
          first.NavigateToCommand.TryExecute(first);
        }
      }
    }

    private bool CanNavigateForward()
    {
      return _forwardHistory.HistoryEntries.Any();
    }
    #endregion Private Methods
  }
}