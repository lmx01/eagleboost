// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-20 10:14 PM

namespace eagleboost.googledrive.Views
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using eagleboost.core.Extensions;
  using eagleboost.googledrive.Contracts;
  using eagleboost.googledrive.Extensions;
  using eagleboost.presentation.Controls.DataGrids;

  /// <summary>
  ///GoogleDriveFileComparer 
  /// </summary>
  public class GoogleDriveFileComparer : IColumnComparer, IComparer<IGoogleDriveFile>
  {
    #region IColumnComparer
    public string Column { get; set; }

    public ListSortDirection Direction { get; set; }
    #endregion IColumnComparer

    #region Private Properties
    private int DirectionFlag
    {
      get { return Direction == ListSortDirection.Ascending ? 1 : -1; }
    }
    #endregion Private Properties

    #region IComparer
    public int Compare(object x, object y)
    {
      return Compare((IGoogleDriveFile) x, (IGoogleDriveFile) y);
    }

    public int Compare(IGoogleDriveFile x, IGoogleDriveFile y)
    {
      if (x == null && y == null)
      {
        return 0;
      }

      if (x == null)
      {
        return 1;
      }

      if (y == null)
      {
        return -1;
      }

      if (x.IsFolder() && y.IsFolder())
      {
        return DirectionFlag * x.Name.CompareNoCase(y.Name);
      }

      if (x.IsFolder())
      {
        return -1;
      }

      if (y.IsFolder())
      {
        return 1;
      }

      return DirectionFlag * string.Compare(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase);
    }
    #endregion IComparer
  }
}