namespace eagleboost.googledrive.Views
{
  using System.Collections;
  using System.Windows;
  using eagleboost.presentation.Controls.TreeView;

  /// <summary>
  /// Interaction logic for GoogleDriveTreeView.xaml
  /// </summary>
  public partial class GoogleDriveTreeView
  {
    #region Statics
    public const string TreeItemDataTemplate = "TreeItemDataTemplate";
    public const string TreeItemDataTemplateWithFrequency = "TreeItemDataTemplateWithFrequency";
    #endregion Statics
    
    public GoogleDriveTreeView()
    {
      InitializeComponent();
      DriveTree.SelectedItemChanged += HandleSelectedItemChanged;
    }

    #region Dependency Properties
    #region EnableFrequentFiles
    public static readonly DependencyProperty EnableFrequentFilesProperty = DependencyProperty.Register(
      "EnableFrequentFiles", typeof(bool), typeof(GoogleDriveTreeView));

    public bool EnableFrequentFiles
    {
      get { return (bool) GetValue(EnableFrequentFilesProperty); }
      set { SetValue(EnableFrequentFilesProperty, value); }
    }
    #endregion EnableFrequentFiles

    #region ItemsSource
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
      "ItemsSource", typeof(IEnumerable), typeof(GoogleDriveTreeView), new PropertyMetadata(default(IEnumerable)));

    public IEnumerable ItemsSource
    {
      get { return (IEnumerable) GetValue(ItemsSourceProperty); }
      set { SetValue(ItemsSourceProperty, value); }
    }
    #endregion ItemsSource

    #region SelectedItem
    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
      "SelectedItem", typeof(ITreeNode), typeof(GoogleDriveTreeView));

    public ITreeNode SelectedItem
    {
      get { return (ITreeNode)GetValue(SelectedItemProperty); }
      set { SetValue(SelectedItemProperty, value); }
    }
    #endregion SelectedItem
    #endregion Dependency Properties

    #region Overrides

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      
      if (EnableFrequentFiles)
      {
        DriveTree.ItemTemplate = (HierarchicalDataTemplate)FindResource(TreeItemDataTemplateWithFrequency);
      }
      else
      {
        DriveTree.ItemTemplate = (HierarchicalDataTemplate)FindResource(TreeItemDataTemplate);
      }
    }

    #endregion Overrides
    
    #region Event Handlers
    private void HandleSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      SelectedItem = (ITreeNode)e.NewValue;
    }
   #endregion Event Handlers
  }
}
