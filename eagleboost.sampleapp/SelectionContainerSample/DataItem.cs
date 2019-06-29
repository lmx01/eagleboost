// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 12:17 PM

namespace eagleboost.sampleapp.SelectionContainerSample
{
  using System;

  public class DataItem : IComparable<DataItem>, IComparable
  {
    public DataItem(string name)
    {
      Name = name;
    }

    public string Name { get; set; }

    public override string ToString()
    {
      return Name;
    }

    public int CompareTo(object obj)
    {
      return CompareTo(obj as DataItem);
    }

    public int CompareTo(DataItem other)
    {
      if (ReferenceEquals(this, other)) return 0;
      if (ReferenceEquals(null, other)) return 1;
      return string.Compare(Name, other.Name, StringComparison.Ordinal);
    }
  }
}