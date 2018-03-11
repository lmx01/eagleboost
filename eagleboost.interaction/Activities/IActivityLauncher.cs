namespace eagleboost.interaction.Activities
{
  using System;
  using System.Threading.Tasks;

  public interface IActivityLauncher
  {
    #region Methods
    Task<TResult> StartActivityAsync<TResult>(Type activityType, params object[] args);
    #endregion Methods
  }
}