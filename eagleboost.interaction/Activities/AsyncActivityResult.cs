// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-29 5:18 PM

namespace eagleboost.interaction.Activities
{
  using System.Diagnostics;

  [DebuggerDisplay("Result={Result}, Confirmed={IsConfirmed}")]
  public class AsyncActivityResult<T>
  {
    public readonly T Result;

    public readonly bool IsConfirmed;

    #region ctors
    public AsyncActivityResult(bool isConfirmed, T result)
    {
      Result = result;
      IsConfirmed = isConfirmed;
    }
    #endregion ctors
  }
}