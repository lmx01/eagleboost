namespace eagleboost.shell.Shell.ViewModels
{
  using System.ComponentModel;
  using eagleboost.core.ComponentModel;

  public class WindowsShellTreeViewOptions : NotifyPropertyChangedBase
  {
    #region Statics
    public static readonly PropertyChangedEventArgs ShowFilesArgs = GetChangedArgs<WindowsShellTreeViewOptions>(o => o.ShowFiles);
    public static readonly PropertyChangedEventArgs FileFiltersArgs = GetChangedArgs<WindowsShellTreeViewOptions>(o => o.FileFilters);
    public static readonly PropertyChangedEventArgs FilterTextArgs= GetChangedArgs<WindowsShellTreeViewOptions>(o => o.FilterText);
    #endregion Statics

    #region Declarations
    private bool _showFiles;
    private string[] _fileFilters = new string[0];
    private string _filterText;
    #endregion Declarations

    public bool ShowFiles
    {
      get { return _showFiles; }
      set { SetValue(ref _showFiles, value); }
    }

    public string[] FileFilters
    {
      get { return _fileFilters; }
      set { SetValue(ref _fileFilters, value); }
    }

    public string FilterText
    {
      get { return _filterText; }
      set { SetValue(ref _filterText, value); }
    }
  }
}