// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 19 9:07 PM

namespace eagleboost.shell.FileSystems.Models
{
  /// <summary>
  /// FileFrequency
  /// </summary>
  public class FileFrequency
  {
    public FileFrequency(string id, int frequency)
    {
      Id = id;
      Frequency = frequency;
    }

    public string Id { get; set; }

    public int Frequency { get; set; }

    public override string ToString()
    {
      return Id + ": " + Frequency;
    }
  }
}