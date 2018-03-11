using System;

namespace eagleboost.core.Extensions
{
  public static class DoubleExt
  {
    public static readonly double Epsilon = 2.22044604925031E-15;
    
    public static bool IsZero(this double d)
    {
      return Math.Abs(d - 0) < Epsilon;
    }

    public static bool Equals(this double d, double v)
    {
      return Math.Abs(d - v) < Epsilon;
    }
  }
}