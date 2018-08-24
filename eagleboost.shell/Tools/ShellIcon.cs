// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-02-12 10:31 PM

namespace eagleboost.shell.Tools
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Interactivity;
  using eagleboost.core.Extensions;

  public class ShellIcon : Behavior<Image>
  {
    #region Dependency Properties
    public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register(
      "FileName", typeof(string), typeof(ShellIcon), new PropertyMetadata(OnFileNameChanged));

    public string FileName
    {
      get { return (string) GetValue(FileNameProperty); }
      set { SetValue(FileNameProperty, value); }
    }

    private static void OnFileNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((ShellIcon)obj).OnFileNameChanged();
    }
    #endregion Dependency Properties

    #region Public Properties
    public bool Large { get; set; }
    #endregion Public Properties

    #region Overrides
    protected override void OnAttached()
    {
      base.OnAttached();

      TryUpdateImage();
    }
    #endregion Overrides

    #region Private Methods
    private void OnFileNameChanged()
    {
      TryUpdateImage();
    }

    private void TryUpdateImage()
    {
      var image = AssociatedObject;
      var fileName = FileName;
      if (image == null || fileName.IsNullOrEmpty() || !fileName.Contains("."))
      {
        return;
      }

      image.Source = ShellIconManager.FindIconForFilename(FileName, Large);
    }
    #endregion Private Methods
  }
}