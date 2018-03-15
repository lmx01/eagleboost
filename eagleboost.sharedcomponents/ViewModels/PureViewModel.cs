// Author : Shuo Zhang
// 
// Creation :2018-03-12 18:04

namespace eagleboost.sharedcomponents.ViewModels
{
  using System.Windows.Input;
  using eagleboost.core.Commands;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.ComponentModel.AutoNotify;
  using eagleboost.core.Contracts.AutoNotify;
  using eagleboost.core.Extensions;

  public class PureViewModel : NotifyPropertyChangedBase, IAutoNotify, IMethodAutoInvoked
  {
    #region Declarations
    private ICommand _okCommand;
    #endregion Declarations

    #region Public Properties
    public virtual string FirstName { get; set; }

    public virtual string LastName { get; set; }

    public virtual int Age { get; set; }

    public string FullName
    {
      get { return string.Format("{0} {1}", FirstName, LastName); }
    }

    public string Error
    {
      get
      {
        if (FirstName.IsNullOrEmpty())
        {
          return "FirstName cannot be empty";
        }

        if (LastName.IsNullOrEmpty())
        {
          return "LastName cannot be empty";
        }

        return null;
      }
    }

    public ICommand OkCommand
    {
      get { return _okCommand ?? (_okCommand = new NotifiableCommand(() => { }, () => FullName.HasValue() && FullName != " ")); }
    }
    #endregion Public Properties

    public void OnAgeChanged(InvokeContext context)
    {
    }

    public void OnAgeChanged2()
    {
    }

    public IAutoNotifyConfig Config
    {
      get { return AutoNotifyConfig<PureViewModel>.Instance; }
    }

    public event MethodInvokedEventHandler MethodInvoked;

    public void OnMethodInvoked(string name, InvokeContext context)
    {
      var handler = MethodInvoked;
      if (handler != null)
      {
        handler(this, new MethodInvokedEventArgs(name, context));
      }
    }
  }
}