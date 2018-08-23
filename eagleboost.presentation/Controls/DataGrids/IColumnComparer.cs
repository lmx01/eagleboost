// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-20 10:05 PM

namespace eagleboost.presentation.Controls.DataGrids
{
  using System.Collections;
  using System.ComponentModel;

  public interface IColumnComparer : IComparer
  {
    #region Properties
    string Column { get; set; }

    ListSortDirection Direction { get; set; }
    #endregion Properties
  }
}