// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 18 9:21 PM

namespace eagleboost.core.Collections
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  /// <summary>
  /// ComparerBase
  /// </summary>
  public abstract class ComparerBase<T> : IComparer, IComparer<T> where T : class
  {
    #region IComparer
    int IComparer.Compare(object x, object y)
    {
      return Compare(x as T, y as T);
    }
    #endregion IComparer

    #region IComparer<T> 
    public int Compare(T x, T y)
    {
      return CompareImpl(x, y);
    }
    #endregion IComparer<T> 

    #region Virtuals
    protected abstract int CompareImpl(T x, T y);
    #endregion Virtuals
  }
}