// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-24 12:39 AM

namespace eagleboost.googledrive.Behaviors
{
  using System;
  using System.Threading.Tasks;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Interactivity;
  using eagleboost.core.Extensions;
  using eagleboost.googledrive.Contracts;
  using eagleboost.presentation.Extensions;
  using eagleboost.presentation.Tools;
  using eagleboost.shell.Tools;

  public class GoogleDriveFileIcon : Behavior<Image>
  {
    #region Dependency Properties
    public static readonly DependencyProperty FileProperty = DependencyProperty.Register(
      "File", typeof(IGoogleDriveFile), typeof(GoogleDriveFileIcon), new PropertyMetadata(OnFileChanged));

    public IGoogleDriveFile File
    {
      get { return (IGoogleDriveFile)GetValue(FileProperty); }
      set { SetValue(FileProperty, value); }
    }

    private static void OnFileChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((GoogleDriveFileIcon)obj).OnFileChanged();
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
    private void OnFileChanged()
    {
      TryUpdateImage();
    }

    private void TryUpdateImage()
    {
      var image = AssociatedObject;
      var file = File;
      if (image == null || file == null || file is IGoogleDriveFolder)
      {
        return;
      }

      if (file.IconLink.HasValue())
      {
        var d = image.Dispatcher;
        RemoteIconManager.GetIconAsync(file.IconLink).ContinueWith(t => d.CheckedInvoke(() => image.Source = t.Result));
      }
      else
      {
        image.Source = ShellIconManager.FindIconForFilename(file.Name, Large);
      }
    }
    #endregion Private Methods
  }
}