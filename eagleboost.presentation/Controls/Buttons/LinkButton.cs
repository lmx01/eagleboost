// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 11 8:53 PM

namespace eagleboost.presentation.Controls.Buttons
{
  using System;
  using System.Diagnostics;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Documents;
  using System.Windows.Navigation;
  using eagleboost.presentation.Extensions;

  /// <summary>
  /// LinkButton - The simplest LinkButton implementation
  /// </summary>
  public class LinkButton : Control
  {
    #region ctors
    static LinkButton()
    {
      FrameworkElementExt.OverrideDefaultStyleKey();
    }
    #endregion ctors

    #region Dependency Properties
    #region Content
    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
      "Content", typeof(string), typeof(LinkButton));

    public string Content
    {
      get { return (string)GetValue(ContentProperty); }
      set { SetValue(ContentProperty, value); }
    }
    #endregion Content

    #region Link
    public static readonly DependencyProperty LinkProperty = DependencyProperty.Register(
      "Link", typeof(Uri), typeof(LinkButton));

    public Uri Link
    {
      get { return (Uri) GetValue(LinkProperty); }
      set { SetValue(LinkProperty, value); }
    }
    #endregion Link
    #endregion Dependency Properties

    #region Overrides
    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();

      var hyperlink = GetTemplateChild("PART_Hyperlink") as Hyperlink;
      if (hyperlink != null)
      {
        hyperlink.RequestNavigate += HandleRequestNavigate;
      }
    }

    private static void HandleRequestNavigate(object sender, RequestNavigateEventArgs e)
    {
      Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
      e.Handled = true;
    }
    #endregion Overrides
  }
}