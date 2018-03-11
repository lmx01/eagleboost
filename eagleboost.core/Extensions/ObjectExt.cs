namespace eagleboost.core.Extensions
{
  public static class ObjectExt
  {
    public static string NullableToString(this object obj)
    {
      return obj != null ? obj.ToString() : null;
    }
  }
}