// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 02 7:31 PM

namespace eagleboost.core.ComponentModel
{
  public class SimpleDisplayItem : IDisplayItem
  {
    public SimpleDisplayItem(string id, string name)
    {
      Id = id;
      DisplayName = name;
    }

    public string Id { get; private set; }

    public string DisplayName { get; private set; }
  }
}