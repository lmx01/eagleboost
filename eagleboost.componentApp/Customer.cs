// Author : Shuo Zhang
// 
// Creation :2018-03-02 22:18

namespace eagleboost.componentApp
{
  using eagleboost.core.ComponentModel;

  public class Customer : NotifyPropertyChangedBase
  {
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }

    public string FullName
    {
      get { return string.Format("{0} {1}", FirstName, LastName); }
    }
    public virtual int Age { get; set; }
  }
}