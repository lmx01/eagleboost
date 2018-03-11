namespace eagleboost.interaction.Activities
{
  using System;
  using System.Threading.Tasks;

  public class ActivityLauncher : IActivityLauncher
  {
    #region IActivityLauncher
    public Task<TResult> StartActivityAsync<TResult>(Type activityType, params object[] args)
    {
      var activity = (Activity)Activator.CreateInstance(activityType);
      return (Task<TResult>)activity.StartActivityAsync(args);
    }
    #endregion IActivityLauncher 
  }
}