// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 07 6:44 PM

namespace eagleboost.googledrive.Models
{
  using eagleboost.googledrive.Contracts;

  /// <summary>
  /// GoogleDriveActivity
  /// </summary>
  public class GoogleDriveActivity
  {
    public GoogleDriveActivity(string user, string action, string name, string id, IGoogleDriveFile file)
    {
      User = user;
      Action = action;
      Name = name;
      Id = id;
      File = file;
    }

    public readonly string User;

    public readonly string Action;

    public readonly string Name;

    public readonly string Id;

    public readonly IGoogleDriveFile File;

    public override string ToString()
    {
      return string.Format("{0} {1}, {2}[{3}]", User, Action, Name, Id);
    }
  }
}