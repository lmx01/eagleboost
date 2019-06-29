// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 3:48 PM

namespace eagleboost.presentation.Controls.Buttons
{
  using System;
  using System.Windows;
  using System.Windows.Input;

  /// <summary>
  /// CommandHelpers
  /// </summary>
  internal class CommandHelpers<T> where T : UIElement, ICommandSource
  {
    #region Declarations
    private readonly T _source;
    #endregion Declarations

    #region ctors
    public CommandHelpers(T source)
    {
      _source = source;
    }
    #endregion ctors

    #region Public Properties
    public bool CanExecute { get; private set; }
    #endregion Public Properties

    #region Public Methods
    public void OnCommandChanged(ICommand oldCommand, ICommand newCommand)
    {
      if (oldCommand != null)
      {
        UnhookCommand(oldCommand);
      }

      if (newCommand != null)
      {
        HookCommand(newCommand);
      }
    }

    public void OnCommandParameterChanged(object oldValue, object newValue)
    {
      UpdateCanExecute();
    }
    #endregion Public Methods

    private void UnhookCommand(ICommand command)
    {
      CanExecuteChangedEventManager.RemoveHandler(command, OnCanExecuteChanged);
      UpdateCanExecute();
    }

    private void HookCommand(ICommand command)
    {
      CanExecuteChangedEventManager.AddHandler(command, OnCanExecuteChanged);
      UpdateCanExecute();
    }

    private void OnCanExecuteChanged(object sender, EventArgs e)
    {
      UpdateCanExecute();
    }

    private void UpdateCanExecute()
    {
      if (_source.Command != null)
      {
        CanExecute = CanExecuteCommandSource(_source);
      }
      else
      {
        CanExecute = true;
      }

      _source.CoerceValue(UIElement.IsEnabledProperty);
    }

    internal bool CanExecuteCommandSource(ICommandSource commandSource)
    {
      var command = commandSource.Command;
      if (command == null)
      {
        return false;
      }

      var commandParameter = commandSource.CommandParameter;
      return command.CanExecute(commandParameter);
    }
  }
}