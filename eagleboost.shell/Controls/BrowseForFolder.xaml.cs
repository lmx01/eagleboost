namespace eagleboost.shell.Controls
{
  using System.Windows;
  using Microsoft.WindowsAPICodePack.Dialogs;
  using Microsoft.WindowsAPICodePack.Shell;

  /// <summary>
  /// Interaction logic for BrowseForFolder.xaml
  /// </summary>
  public partial class BrowseForFolder
  {
    public BrowseForFolder()
    {
      InitializeComponent();
    }

    public static readonly DependencyProperty FolderPathProperty = DependencyProperty.Register(
      "FolderPath", typeof(string), typeof(BrowseForFolder), new PropertyMetadata(default(string)));

    public string FolderPath
    {
      get { return (string)GetValue(FolderPathProperty); }
      set { SetValue(FolderPathProperty, value); }
    }

    private void HandleBrowseButtonClick(object sender, RoutedEventArgs e)
    {
      var cfd = new CommonOpenFileDialog
      {
        EnsureReadOnly = true,
        IsFolderPicker = true,
        AllowNonFileSystemItems = true
      };

      if (cfd.ShowDialog() == CommonFileDialogResult.Ok)
      {
        ShellContainer shellContainer = null;

        try
        {
          // Try to get a valid selected item
          shellContainer = (ShellContainer)cfd.FileAsShellObject;
        }
        catch
        {
          MessageBox.Show("Could not create a ShellObject from the selected item");
        }

        FolderPath = shellContainer.ParsingName;
      }
    }
  }
}
