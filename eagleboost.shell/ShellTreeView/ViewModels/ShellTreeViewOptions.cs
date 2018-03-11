namespace eagleboost.shell.ShellTreeView.ViewModels
{
  using System.ComponentModel;
  using eagleboost.core.ComponentModel;

  public class ShellTreeViewOptions : NotifyPropertyChangedBase
  {
    #region Statics
    public static readonly PropertyChangedEventArgs ShowFilesArgs = GetChangedArgs<ShellTreeViewOptions>(o => o.ShowFiles);
    public static readonly PropertyChangedEventArgs FileFiltersArgs = GetChangedArgs<ShellTreeViewOptions>(o => o.FileFilters);
    public static readonly PropertyChangedEventArgs FilterTextArgs= GetChangedArgs<ShellTreeViewOptions>(o => o.FilterText);
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