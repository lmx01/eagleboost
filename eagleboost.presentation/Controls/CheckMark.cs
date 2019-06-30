// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-22 11:07 PM

namespace eagleboost.presentation.Controls
{
  using System.Windows.Controls;
  using eagleboost.presentation.Extensions;

  /// <summary>
  /// CheckMark
  /// </summary>
  public class CheckMark : CheckBox
  {
    static CheckMark()
    {
      FrameworkElementExt.OverrideDefaultStyleKey();
    }
  }
}
