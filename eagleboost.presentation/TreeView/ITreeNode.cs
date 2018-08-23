﻿// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-19 11:49 PM

namespace eagleboost.presentation.TreeView
{
  using System.Collections.Generic;
  using System.ComponentModel;

  public interface ITreeNode : INotifyPropertyChanged
  {
    #region Properties
    string Name { get; }

    object DataItem { get; }

    IReadOnlyCollection<ITreeNode> Children { get; }

    bool IsExpanded { get; set; }

    bool IsSelected { get; set; }
    #endregion    
  }
}