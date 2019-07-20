// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 18 11:09 PM

namespace eagleboost.core.Collections
{
  using System;
  using eagleboost.core.Utils;

  /// <summary>
  /// PriorityComparer
  /// </summary>
  public class PriorityComparer<T> : ComparerBase<T> where T : class
  {
    #region Declarations
    private readonly Func<T, int> _priorityFunc;
    #endregion Declarations

    #region ctors
    public PriorityComparer(Func<T, int> priorityFunc)
    {
      _priorityFunc = ArgValidation.ThrowIfNull(priorityFunc, "priorityFunc");
    }
    #endregion ctors

    #region Overrides
    protected override int CompareImpl(T x, T y)
    {
      var xPriority = _priorityFunc(x);
      var yPriority = _priorityFunc(y);

      if (xPriority > 0)
      {
        if (yPriority > 0)
        {
          return xPriority - yPriority;
        }

        return -1;
      }

      if (yPriority > 0)
      {
        return 1;
      }

      return 0;
    }
    #endregion Overrides 
  }
}