// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-21 3:55 PM

namespace eagleboost.googledrive.Contracts
{
  using System.Collections.Generic;
  using eagleboost.shell.FileSystems.Contracts;

  /// <summary>
  /// IGoogleDriveFile
  /// </summary>
  public interface IGoogleDriveFile : IFile
  {
    bool OwnedByMe { get; }

    string Owners { get; }

    IDictionary<string, string> AppProperties { get; }
  }
}