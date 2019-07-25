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
  using eagleboost.core.Extensions;
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
      "Link", typeof(string), typeof(LinkButton), new PropertyMetadata(OnLinkChanged));

    public string Link
    {
      get { return (string) GetValue(LinkProperty); }
      set { SetValue(LinkProperty, value); }
    }

    private static void OnLinkChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((LinkButton)obj).OnLinkChanged();
    }
    #endregion Link
    #endregion Dependency Properties

    #region Private Properties
    private Hyperlink Hyperlink
    {
      get { return GetTemplateChild("PART_Hyperlink") as Hyperlink; }
    }
    #endregion Private Properties

    #region Overrides
    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();

      var hyperlink = Hyperlink;
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

    #region Private Methods

    private void OnLinkChanged()
    {
      var link = Link;
      var hyperlink = Hyperlink;
      if (link != null && hyperlink != null)
      {
        hyperlink.NavigateUri = new Uri(link);
      }
    }
    #endregion Private Methods
  }
}