// Author : Shuo Zhang
// 
// Creation :2018-03-16 14:44

namespace eagleboost.sharedcomponents.ViewModels
{
  using eagleboost.core.Contracts.AutoComposite;
  using eagleboost.sharedcomponents.Contracts;

  public class CompositeSource : IPersonInfo
  {
    public string FirstName
    {
      get { return "FirstName"; }
    }

    public string LastName
    {
      get { return "LastName"; }
    }

    public int Age
    {
      get { return 0; }
    }
  }

  public class AutoComposite : IPersonInfo, IAutoComposite<IPersonInfo>
  {
    public IPersonInfo CompositeSource { get; set; }

    public virtual string FirstName { get; private set; }

    public virtual string LastName { get; private set; }

    public virtual int Age { get; private set; }
  }
}