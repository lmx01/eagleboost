namespace eagleboost.interaction.Activities
{
  using System;
  using eagleboost.presentation.Contracts;

  public abstract class DialogActivity<TResult> : Activity<TResult>
  {
    #region Overrides
    protected override void OnStartActivity()
    {
      DispatcherProvider.CheckedInvoke(() =>
      {
        try
        {
          ShowDialog();
        }
        catch (Exception e)
        {
          Completion.TrySetException(e);
        }
      });
    }
    #endregion Overrides

    #region Virtuals
    protected abstract void ShowDialog();
    #endregion Virtuals
  }
}