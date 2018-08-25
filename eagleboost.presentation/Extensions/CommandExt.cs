// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-25 12:25 AM

namespace eagleboost.presentation.Extensions
{
  using System.Windows.Input;

  public static class CommandExt
  {
    public static bool TryExecute(this ICommand command, object commandParameter)
    {
      if (command.CanExecute(commandParameter))
      {
        command.Execute(commandParameter);
        return true;
      }

      return false;
    }
  }
}