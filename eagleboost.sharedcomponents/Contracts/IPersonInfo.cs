// Author : Shuo Zhang
// 
// Creation :2018-03-16 14:46

namespace eagleboost.sharedcomponents.Contracts
{
  public interface IPersonInfo
  {
    string FirstName { get; }

    string LastName { get; }

    int Age { get; }
  }
}