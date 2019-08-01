// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 31 9:49 PM

namespace eagleboost.presentation.Controls.MessageBoxes
{
  using System.Windows;
  using eagleboost.presentation.Contracts;

  /// <summary>
  /// MessageBoxViewModel
  /// </summary>
  public class MessageBoxViewModel : IHeader
  {
    #region Public Properties
    public string Header { get; set; }

    public string Text { get; set; }

    public MessageBoxButton MessageBoxButton { get; set; }

    public MessageBoxImage MessageBoxImage { get; set; }

    public MessageBoxResult Result { get; set; }
    #endregion Public Properties
  }
}