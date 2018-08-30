// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-29 5:30 PM

namespace eagleboost.interaction.Activities
{
  using System.Threading.Tasks;
  using Unity.Attributes;

  /// <summary>
  /// AsyncActivity
  /// </summary>
  /// <typeparam name="TActivity"></typeparam>
  /// <typeparam name="T"></typeparam>
  public class AsyncActivity<TActivity, T> : IAsyncActivity<TActivity, T> where TActivity : class
  {
    #region Components
    [Dependency]
    public IActivityLauncher Launcher { get; set; }
    #endregion Components

    #region IAsyncActivity
    public Task<AsyncActivityResult<T>> StartAsync(params object[] parameters)
    {
      return Launcher.StartActivityAsync<AsyncActivityResult<T>>(typeof(TActivity), parameters);
    }
    #endregion IAsyncActivity
  }
}