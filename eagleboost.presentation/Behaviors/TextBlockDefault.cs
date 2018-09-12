// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-11 9:02 PM

namespace eagleboost.presentation.Behaviors
{
  using System;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Interactivity;
  using eagleboost.core.Extensions;

  /// <summary>
  /// TextBlockDefault
  /// </summary>
  public class TextBlockDefault : Behavior<TextBlock>
  {
    #region Public Properties
    public string StringFormat { get; set; }

    public string DefaultText { get; set; }
    #endregion Public Properties

    #region Dependency Properties
    #region Source
    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
      "Source", typeof(object), typeof(TextBlockDefault), new PropertyMetadata(OnSourceChanged));

    public object Source
    {
      get { return GetValue(SourceProperty); }
      set { SetValue(SourceProperty, value); }
    }

    private static void OnSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((TextBlockDefault)obj).OnSourceChanged();
    }
    #endregion Source
    #endregion Dependency Properties

    #region Overrides
    protected override void OnAttached()
    {
      base.OnAttached();

      var textBlock= AssociatedObject;
      textBlock.SetValue(TextBlock.TextProperty, DefaultText);
      OnSourceChanged();
    }
    #endregion Overrides

    #region Private Methods
    private void OnSourceChanged()
    {
      var textBlock = AssociatedObject;
      if (textBlock != null && Source != null)
      {
        if (StringFormat.HasValue())
        {
          var content = string.Format(StringFormat, Source);
          textBlock.SetValue(TextBlock.TextProperty, content);
        }
        else
        {
          textBlock.SetValue(TextBlock.TextProperty, Source.ToString());
        }
      }
    }
    #endregion Private Methods
  }
}