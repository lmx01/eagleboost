// Author : Shuo Zhang
// 
// Creation :2018-02-12 09:19

namespace eagleboost.presentation.Behaviors
{
  using System.Windows;
  using System.Windows.Interactivity;
  using eagleboost.core.Extensions;
  using Microsoft.Toolkit.Wpf.UI.Controls;

  /// <summary>
  /// WebViewNavigate
  /// </summary>
  public class WebViewNavigate : Behavior<WebView>
  {
    #region Dependency Properties
    public static readonly DependencyProperty UrlProperty = DependencyProperty.Register(
      "Url", typeof(string), typeof(WebViewNavigate), new PropertyMetadata(OnUrlChanged));

    public string Url
    {
      get { return (string) GetValue(UrlProperty); }
      set { SetValue(UrlProperty, value); }
    }

    private static void OnUrlChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((WebViewNavigate) obj).OnUrlChanged();
    }
    #endregion Dependency Properties

    #region Private Methods
    private void OnUrlChanged()
    {
      var url = Url;
      var webView = AssociatedObject;
      if (url.IsNullOrEmpty())
      {
        webView.Navigate("about:blank");
      }
      else
      {
        webView.Navigate(url);
      }
    }
    #endregion Private Methods
  }
}