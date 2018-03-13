// Author : Shuo Zhang
// 
// Creation :2018-03-12 22:27

namespace eagleboost.core.Exceptions
{
  using System;

  public class InvalidTypeException : Exception
  {
    public static InvalidTypeException Create<T>(string message = null)
    {
      return new InvalidTypeException(typeof(T), message);
    }

    public InvalidTypeException(Type type, string message = null) : base(message)
    {
      Type = type;
    }

    public readonly Type Type;
  }
}