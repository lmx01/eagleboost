// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 18 11:32 PM

namespace eagleboost.googledrive.Extensions
{
  using eagleboost.core.Collections;
  using eagleboost.core.Extensions;
  using eagleboost.googledrive.Contracts;
  using eagleboost.presentation.Controls.TreeView;

  /// <summary>
  /// GoogleDriveFileNameComparer
  /// </summary>
  public class GoogleDriveFileNameComparer : ComparerBase<TreeNodeContainer>
  {
    #region Overrides
    protected override int CompareImpl(TreeNodeContainer x, TreeNodeContainer y)
    {
      if (x == null)
      {
        return y == null ? 0 : -1;
      }

      if (y == null)
      {
        return 1;
      }

      var xFile = (IGoogleDriveFile) x.DataItem;
      var yFile = (IGoogleDriveFile) y.DataItem;

      var r = xFile.Name.CompareNoCase(yFile.Name);
      if (r == 0)
      {
        return xFile.Id.CompareNoCase(yFile.Id);
      }

      return r;
    }
    #endregion Overrides 
  }
}