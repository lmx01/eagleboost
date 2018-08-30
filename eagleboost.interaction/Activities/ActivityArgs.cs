namespace eagleboost.interaction.Activities
{
  using System.Collections.Generic;

  public class ActivityArgs : List<object>
  {
    #region ctors
    public ActivityArgs(IEnumerable<object> source) : base(source)
    {
    }
    #endregion ctors

    #region Public Methods
    public T GetArgs<T>() where T : class
    {
      foreach (var arg in this)
      {
        if (arg.GetType() == typeof(T))
        {
          return (T)arg;
        }
      }

      foreach (var arg in this)
      {
        var variable = arg as T;
        if (variable != null)
        {
          return variable;
        }
      }

      return null;
    }
    #endregion Public Methods
  }
}