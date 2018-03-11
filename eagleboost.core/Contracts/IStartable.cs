namespace eagleboost.core.Contracts
{
  public interface IStartable
  {
    bool IsStarted { get; }

    void Start();
  }
}