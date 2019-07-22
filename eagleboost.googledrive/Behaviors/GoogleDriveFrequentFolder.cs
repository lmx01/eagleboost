// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 21 9:35 PM

namespace eagleboost.googledrive.Behaviors
{
  using System.Linq;
  using System.Windows;
  using System.Windows.Controls.Primitives;
  using System.Windows.Data;
  using System.Windows.Documents;
  using System.Windows.Interactivity;
  using System.Windows.Media;
  using eagleboost.core.Collections;
  using eagleboost.googledrive.Contracts;
  using eagleboost.presentation.Behaviors.SelectionContainer;

  /// <summary>
  /// GoogleDriveFrequentFolder
  /// </summary>
  public class GoogleDriveFrequentFolder : Behavior<FrameworkElement>
  {
    #region Dependency Properties
    #region SelectionContainer
    public static readonly DependencyProperty SelectionContainerProperty = DependencyProperty.Register(
      "SelectionContainer", typeof(ISelectionContainer), typeof(GoogleDriveFrequentFolder), new PropertyMetadata(OnSelectionContainerChanged));

    public ISelectionContainer SelectionContainer
    {
      get { return (ISelectionContainer)GetValue(SelectionContainerProperty); }
      set { SetValue(SelectionContainerProperty, value); }
    }

    private static void OnSelectionContainerChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((GoogleDriveFrequentFolder)obj).OnSelectionContainerChanged();
    }
    #endregion SelectionContainer

    #region DataItem
    public static readonly DependencyProperty DataItemProperty = DependencyProperty.Register(
      "DataItem", typeof(IGoogleDriveFile), typeof(GoogleDriveFrequentFolder), new PropertyMetadata(OnDataItemChanged));

    public IGoogleDriveFile DataItem
    {
      get { return (IGoogleDriveFile)GetValue(DataItemProperty); }
      set { SetValue(DataItemProperty, value); }
    }

    private static void OnDataItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((GoogleDriveFrequentFolder)obj).UpdateFrequentItem();
    }
    #endregion DataItem
    #endregion Dependency Properties

    #region Event Handlers

    private void OnSelectionContainerChanged()
    {
      var c = SelectionContainer;
      if (c != null)
      {
        c.ItemsSelected += HandleItemsSelected;
      }
      UpdateFrequentItem();
    }

    private void HandleItemsSelected(object sender, ItemsSelectedEventArgs e)
    {
      var item = e.Items.OfType<IGoogleDriveFile>().FirstOrDefault();
      if (item != null)
      {
        UpdateFrequentItem();
      }
    }

    private void UpdateFrequentItem()
    {
      var dataItem = DataItem;
      var c = SelectionContainer;
      var element = AssociatedObject;
      if (dataItem == null || c == null || element == null)
      {
        return;
      }

      if (c[dataItem.Id])
      {
        TextElement.SetForeground(element, Brushes.Red);
      }
    }
    #endregion Event Handlers
  }
}