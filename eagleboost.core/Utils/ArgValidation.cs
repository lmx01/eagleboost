// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 02 7:07 PM

namespace eagleboost.core.Utils
{
  using System;

  public class ArgValidation
  {
    public static T ThrowIfNull<T>(T paramValue, string paramName) where T : class 
    {
      if (paramValue == null)
      {
        throw new ArgumentNullException(paramName);
      }

      return paramValue;
    }
  }
}