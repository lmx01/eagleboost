// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 18 9:06 PM

namespace eagleboost.core.Collections
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using eagleboost.core.Utils;

  /// <summary>
  /// CompositeComparer
  /// </summary>
  public sealed class CompositeComparer<T> : ComparerBase<T> where T : class
  {
    #region Declarations
    private readonly IComparer<T>[] _comparers;
    #endregion Declarations

    #region ctors
    public CompositeComparer(IEnumerable<IComparer<T>> comparers)
    {
      var c = ArgValidation.ThrowIfNull(comparers, "comparers");
      _comparers = c.ToArray();

      if (_comparers.Length == 0)
      {
        throw new ArgumentException("comparers is empty");
      }
    }
    #endregion ctors

    #region Overrides
    protected override int CompareImpl(T x, T y)
    {
      for (var i = 0; i < _comparers.Length; i++)
      {
        var c = _comparers[i];
        var r = c.Compare(x, y);
        if (r != 0)
        {
          return r;
        }
      }

      return 0;
    }
    #endregion IComparer<T> 
  }
}