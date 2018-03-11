namespace eagleboost.interaction.Activities
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using eagleboost.presentation.Contracts;
  using eagleboost.presentation.Tasks;

  public abstract class Activity
  {
    #region Public Properties
    public ActivityArgs ActivityArgs { get; private set; }

    public IDispatcherProvider DispatcherProvider { get; private set; }
    #endregion Public Properties

    #region Methods
    public virtual Task StartActivityAsync(params object[] args)
    {
      ActivityArgs = new ActivityArgs(args);
      DispatcherProvider = new DispatcherProvider();
      return StartActivityAsync();
    }
    #endregion Methods 

    #region Virtuals
    public abstract Task StartActivityAsync();
    #endregion Virtuals
  }
}