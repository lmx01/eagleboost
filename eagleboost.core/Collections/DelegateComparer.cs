// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 18 9:20 PM

namespace eagleboost.core.Collections
{
  using System;
  using eagleboost.core.Utils;

  /// <summary>
  /// DelegateComparer
  /// </summary>
  public class DelegateComparer<T> : ComparerBase<T> where T : class
  {
    #region Declarations
    private readonly Func<T, T, int> _func;
    #endregion Declarations

    #region ctors
    public DelegateComparer(Func<T, T, int> func)
    {
      _func = ArgValidation.ThrowIfNull(func, "func");
    }
    #endregion ctors

    #region Overrides
    protected override int CompareImpl(T x, T y)
    {
      return _func(x, y);
    }
    #endregion Overrides 
  }
}