// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-05 10:49 PM

namespace eagleboost.presentation.Controls.Progress
{
  using System.Collections.ObjectModel;
  using System.Collections.Specialized;
  using System.ComponentModel;
  using System.Windows.Input;
  using eagleboost.core.Commands;
  using eagleboost.core.Contracts;
  using eagleboost.core.Extensions;
  using eagleboost.presentation.Contracts;
  using eagleboost.presentation.Extensions;
  using eagleboost.presentation.Interactivity;
  using eagleboost.presentation.Tasks;
  using Prism.Commands;

  /// <summary>
  /// ProgressViewModel
  /// </summary>
  public class ProgressViewModel : ViewController
  {
    #region Declarations
    private IInvalidatableCommand _pauseCmd;
    private IInvalidatableCommand _resumeCmd;
    private IDispatcherProvider _dispatcher = new DispatcherProvider();
    #endregion Declarations

    #region ctors
    public ProgressViewModel()
    {
      ProgressItems = new ObservableCollection<IProgressItemViewModel>();
      ProgressItems.CollectionChanged += HandleProgressItemsChanged;
      _pauseCmd = new NotifiableCommand<IProgressItemViewModel>(
        i => i.PauseCommand.TryExecute(null),
        i => i != null && i.PauseCommand.CanExecute(null));
      _resumeCmd = new NotifiableCommand<IProgressItemViewModel>(
        i => i.ResumeCommand.TryExecute(null),
        i => i != null && i.ResumeCommand.CanExecute(null));
      CancelTaskCommand = new DelegateCommand<IProgressItemViewModel>(i => i.CancelCommand.TryExecute(null));
    }
    #endregion ctors

    #region Public Properties
    public ObservableCollection<IProgressItemViewModel> ProgressItems { get; private set; }

    public ICommand PauseTaskCommand
    {
      get { return _pauseCmd; }
    }

    public ICommand ResumeTaskCommand
    {
      get { return _resumeCmd; }
    }

    public ICommand CancelTaskCommand { get; private set; }
    #endregion Public Properties

    #region Event Handlers
    private void HandleProgressItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Add)
      {
        var item = (IProgressItemViewModel)e.NewItems[0];
        item.PropertyChanged += HandleProgressItemChanged;
      }
      else if (e.Action == NotifyCollectionChangedAction.Remove)
      {
        var item = (IProgressItemViewModel)e.OldItems[0];
        item.PropertyChanged -= HandleProgressItemChanged;
      }
    }

    private void HandleProgressItemChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.Match<IProgressItemViewModel>(v => v.State) || e.Match<IProgressItemViewModel>(v => v.TimeElapsed))
      {
        _dispatcher.CheckedInvoke(() =>
        {
          _pauseCmd.Invalidate();
          _resumeCmd.Invalidate();
        });
      }
    }
    #endregion Event Handlers
  }
}