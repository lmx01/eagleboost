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

    public static T ThrowIfMismatch<T>(object paramValue, string paramName) where T : class
    {
      var result = paramValue as T;
      if (result == null)
      {
        throw new ArgumentException("Type is not " + typeof(T), "paramValue");
      }

      return result;
    }
  }
}