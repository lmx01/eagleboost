namespace eagleboost.interaction.Activities
{
  using System.Threading.Tasks;

  public abstract class Activity<TResult> : Activity
  {
    #region Public Properties
    public TaskCompletionSource<TResult> Completion { get; } = new TaskCompletionSource<TResult>();
    #endregion Public Properties

    #region Overrides
    public override Task StartActivityAsync()
    {
      return StartActivityAsyncImpl();
    }
    #endregion Overrides

    #region Virtuals
    protected virtual Task<TResult> StartActivityAsyncImpl()
    {
      Task.Run(() => OnStartActivity());
      return Completion.Task;
    }

    protected abstract void OnStartActivity();
    #endregion Virtuals
  }
}