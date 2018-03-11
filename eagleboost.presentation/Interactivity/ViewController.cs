using System;
using System.Collections.Generic;
using System.Windows.Input;
using eagleboost.core.ComponentModel;
using Prism.Commands;

namespace eagleboost.presentation.Interactivity
{
  public class ViewController : NotifyPropertyChangedBase, IViewController
  {
    private readonly List<Action<bool?>> _closeActions = new List<Action<bool?>>();

    public ViewController()
    {
      OkCommand = new DelegateCommand(HandleOk);
      CancelCommand = new DelegateCommand(HandleCancel);
    }

    public void AddCloseAction(Action<bool?> closeAction)
    {
      _closeActions.Add(closeAction);
    }

    public ICommand OkCommand { get; private set; }

    public ICommand CancelCommand { get; private set; }

    private void HandleOk()
    {
      foreach (var closeAction in _closeActions)
      {
        closeAction(true);
      }
    }

    private void HandleCancel()
    {
      foreach (var closeAction in _closeActions)
      {
        closeAction(null);
      }
    }
  }
}