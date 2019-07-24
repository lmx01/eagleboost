// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 23 8:12 AM

namespace eagleboost.googledrive.Views
{
  using eagleboost.presentation.Controls.TreeView;
  using eagleboost.shell.FileSystems.Models;

  /// <summary>
  /// GoogleDrivePopupTreeState
  /// </summary>
  public class GoogleDrivePopupTreeState
  {
    public ITreeNode SelectedItem { get; set; }

    public FileFrequency[] FrequentFiles { get; set; } 
  }
}