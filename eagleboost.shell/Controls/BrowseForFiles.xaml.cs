namespace eagleboost.shell.Controls
{
  using System.Collections;
  using System.Windows;
  using Microsoft.Win32;

  /// <summary>
  /// Interaction logic for BrowseForFiles.xaml
  /// </summary>
  public partial class BrowseForFiles
  {
    public BrowseForFiles()
    {
      InitializeComponent();
    }

    public static readonly DependencyProperty FilePathsProperty = DependencyProperty.Register(
      "FilePaths", typeof(IEnumerable), typeof(BrowseForFiles));

    public IEnumerable FilePaths
    {
      get { return (IEnumerable)GetValue(FilePathsProperty); }
      set { SetValue(FilePathsProperty, value); }
    }

    private void HandleOpenButtonClick(object sender, RoutedEventArgs e)
    {
      var openFileDialog = new OpenFileDialog {Multiselect = true};

      if (openFileDialog.ShowDialog() == true)
      {
        FilePaths = openFileDialog.FileNames;
      }
    }
  }
}
