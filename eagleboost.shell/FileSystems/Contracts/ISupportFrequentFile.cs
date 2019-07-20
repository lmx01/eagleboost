// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 19 9:07 PM

namespace eagleboost.shell.FileSystems.Contracts
{
  using System.Collections.Generic;
  using eagleboost.shell.FileSystems.Models;

  /// <summary>
  /// ISupportFrequentFile
  /// </summary>
  public interface ISupportFrequentFile
  {
    void SetFrequentFiles(IEnumerable<FileFrequency> frequencies);

    IReadOnlyCollection<FileFrequency> GetFrequentFiles();
  }
}