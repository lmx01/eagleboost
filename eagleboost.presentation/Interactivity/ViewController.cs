namespace eagleboost.presentation.Interactivity
{
  using System;
  using System.Collections.Generic;
  using System.Windows.Input;
  using eagleboost.core.ComponentModel;
  using eagleboost.presentation.Commands;

  public class ViewController : NotifyPropertyChangedBase<ViewController>, IViewController
  {
    private readonly List<Action<bool?>> _closeActions = new List<Action<bool?>>();

    public ViewController()
    {
      OkText = "_OK";
      CancelText = "_Cancel";
      OkCommand = new NotifiableCommand(HandleOk, () => CanHandleOk, this);
      CancelCommand = new NotifiableCommand(HandleCancel, () => CanHandleCancel, this);
    }

    public void AddCloseAction(Action<bool?> closeAction)
    {
      _closeActions.Add(closeAction);
    }

    public string OkText { get; set; }

    public string CancelText { get; set; }

    public ICommand OkCommand { get; private set; }

    public ICommand CancelCommand { get; private set; }

    private void HandleOk()
    {
      foreach (var closeAction in _closeActions)
      {
        closeAction(true);
      }
    }

    protected virtual bool CanHandleOk
    {
      get { return true; }
    }

    private void HandleCancel()
    {
      foreach (var closeAction in _closeActions)
      {
        closeAction(null);
      }
    }

    protected virtual bool CanHandleCancel
    {
      get { return true; }
    }
  }
}