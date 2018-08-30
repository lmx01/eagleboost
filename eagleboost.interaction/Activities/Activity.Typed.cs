namespace eagleboost.interaction.Activities
{
  using System.Threading.Tasks;

  public abstract class Activity<TResult> : Activity
  {
    #region Declarations
    private readonly TaskCompletionSource<TResult> _completion = new TaskCompletionSource<TResult>();
    #endregion Declarations

    #region Public Properties
    public TaskCompletionSource<TResult> Completion
    {
      get { return _completion; }
    }
    #endregion Public Properties

    #region Overrides
    public override Task StartActivityAsync()
    {
      return DoStartActivityAsync();
    }
    #endregion Overrides

    #region Virtuals
    protected virtual Task<TResult> DoStartActivityAsync()
    {
      Task.Run(() => OnStartActivity());
      return Completion.Task;
    }

    protected abstract void OnStartActivity();
    #endregion Virtuals
  }
}