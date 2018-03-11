namespace eagleboost.presentation.Behaviors
{
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;
  using System.Windows.Interactivity;
  using eagleboost.presentation.Contracts;
  using eagleboost.presentation.Extensions;

  /// <summary>
  ///TreeViewSelectionChanging 
  /// </summary>
  public class TreeViewSelectionChanging : Behavior<TreeView>
  {
    #region Declarations
    private TreeView _treeView;
    private IPreviewSelectionChange _previewSelectionChange;
    #endregion Declarations

    #region Overrides
    protected override void OnAttached()
    {
      base.OnAttached();

      _treeView = AssociatedObject;
      _treeView.SetupDataContextChanged<IPreviewSelectionChange>(HandleDataContext);
    }

    protected override void OnDetaching()
    {
      _treeView.PreviewMouseLeftButtonDown -= HandlePreviewMouseLeftButtonDown;

      base.OnDetaching();
    }
    #endregion Overrides

    #region Event Handlers
    private void HandleDataContext(IPreviewSelectionChange context)
    {
      if (context != null)
      {
        _previewSelectionChange = context;
        _treeView.PreviewMouseLeftButtonDown += HandlePreviewMouseLeftButtonDown;
      }
    }

    private void HandlePreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var treeView = (TreeView) sender;
      var source = (DependencyObject) e.OriginalSource;
      var treeViewItem = source.FindParent<TreeViewItem>();
      if (treeViewItem != null)
      {
        var current = treeView.SelectedItem;
        var newItem = treeViewItem.DataContext;
        if (current != newItem)
        {
          if (!_previewSelectionChange.PreviewSelectionChange(current, newItem))
          {
            e.Handled = true;
          }
        }
      }
    }
    #endregion Event Handlers
  }
}