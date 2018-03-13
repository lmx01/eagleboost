// Author : Shuo Zhang
// 
// Creation :2018-03-05 22:49

namespace eagleboost.sharedcomponents.ViewModels
{
  using System;
  using System.Linq.Expressions;
  using System.Windows.Input;
  using eagleboost.core.Commands;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.ComponentModel.AutoNotify;
  using eagleboost.core.Extensions;

  public class ViewModel : NotifyPropertyChangedBase<ViewModel>
  {
    #region Statics
    private static readonly Expression<Func<ViewModel, string>>[] NamesSelector = { v => v.FirstName, v => v.LastName };
    #endregion Statics

    #region Declarations
    private ICommand _okCommand;
    #endregion Declarations

    #region ctors
    static ViewModel()
    {
      Invoke(v => v.OnAgeChanged).By(v => v.Age);
      Invoke(v => v.OnAgeChanged2).By(v => v.Age);
      Notify(v => v.FullName).By(NamesSelector);
      Notify(v => v.Error).By(NamesSelector);
      Invalidate(v => v.OkCommand).By(NamesSelector);
    }
    #endregion ctors

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
  }
}