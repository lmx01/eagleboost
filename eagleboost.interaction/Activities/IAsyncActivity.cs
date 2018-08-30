// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-29 5:18 PM

namespace eagleboost.interaction.Activities
{
  using System.Threading.Tasks;

  /// <summary>
  /// IAsyncActivity
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IAsyncActivity<T>
  {
    Task<AsyncActivityResult<T>> StartAsync(params object[] parameters);
  }

  /// <summary>
  /// IAsyncActivity
  /// </summary>
  /// <typeparam name="TActivity"></typeparam>
  /// <typeparam name="T"></typeparam>
  public interface IAsyncActivity<TActivity, T> : IAsyncActivity<T> where TActivity : class
  {
  }

  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="TActivity"></typeparam>
  public interface IVoidAsyncActivity<TActivity> where TActivity : class
  {
    Task<AsyncActivityResult<object>> StartAsync(params object[] parameters);
  }
}