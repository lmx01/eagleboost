// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 22 9:15 PM

namespace eagleboost.sampleapp.Controls
{
  using System.Windows;
  using System.Windows.Controls;
  using eagleboost.UserExperience.Controls;

  /// <summary>
  /// StaThreadTextBlock
  /// </summary>
  public class StaThreadTextBlock : StaThreadContainer
  {
    protected override FrameworkElement CreateControl(object initParams)
    {
      return new TextBlock
      {
        Text = string.Format("I'm running on thread '{0}", ThreadName)
      };
    }
  }
}