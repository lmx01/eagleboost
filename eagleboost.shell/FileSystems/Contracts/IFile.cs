// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-30 9:04 PM

namespace eagleboost.shell.FileSystems.Contracts
{
  using System;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.Types;

  /// <summary>
  /// IFile
  /// </summary>
  public interface IFile : IDisplayItem
  {
    string Name { get; }

    string Type { get; }
    
    long? Size { get; }

    FileSize? FileSize { get; }

    IFolder Parent { get; }

    DateTime? CreatedTime { get; }

    DateTime? ModifiedTime { get; }
  }
}