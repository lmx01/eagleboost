// Author : Shuo Zhang
// 
// Creation :2018-03-12 22:24

namespace eagleboost.core.ComponentModel
{
  using System;

  public class ArgumentGuard
  {
    public static void ThrowIfNull(object obj, string name)
    {
      if (obj == null)
      {
        throw new ArgumentException(string.Format("'{0}' is null", name));
      }
    }
  }
}