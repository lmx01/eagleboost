// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 07 6:44 PM

namespace eagleboost.googledrive.Models
{
  /// <summary>
  /// GoogleDriveActivity
  /// </summary>
  public class GoogleDriveActivity
  {
    public GoogleDriveActivity(string user, string action, string name, string id, string time)
    {
      User = user;
      Action = action;
      Name = name;
      Id = id;
      Time = time;
    }

    public readonly string User;

    public readonly string Action;

    public readonly string Name;

    public readonly string Id;

    public readonly string Time;

    public override string ToString()
    {
      return string.Format("{0}: {1}, {2}, {3}[{4}]", Time, User, Action, Name, Id);
    }
  }
}