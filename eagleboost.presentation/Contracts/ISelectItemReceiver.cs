// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-25 12:21 AM

namespace eagleboost.presentation.Contracts
{
  using System.Windows.Input;

  /// <summary>
  /// ISelectItemReceiver
  /// </summary>
  public interface ISelectItemReceiver
  {
    ICommand SelectItemCommand { get; }
  }
}