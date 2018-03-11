namespace eagleboost.shell.Controls
{
  using System.Windows;
  using Microsoft.Win32;

  /// <summary>
  /// Interaction logic for BrowseForFile.xaml
  /// </summary>
  public partial class BrowseForFile
  {
    public BrowseForFile()
    {
      InitializeComponent();
    }

    public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register(
      "FilePath", typeof(string), typeof(BrowseForFile), new PropertyMetadata(default(string)));

    public string FilePath
    {
      get { return (string) GetValue(FilePathProperty); }
      set { SetValue(FilePathProperty, value); }
    }

    private void HandleOpenButtonClick(object sender, RoutedEventArgs e)
    {
      var openFileDialog = new OpenFileDialog();
      var initialDir = FilePath != null ? System.IO.Path.GetDirectoryName(FilePath) : null;
      if (initialDir != null)
      {
        openFileDialog.InitialDirectory = initialDir;
      }

      if (openFileDialog.ShowDialog() == true)
      {
        FilePath = openFileDialog.FileName;
      }
    }
  }
}
