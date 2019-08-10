namespace eagleboost.core.Data
{
  /// <summary>
  /// BooleanBoxes
  /// </summary>
  public static class BooleanBoxes
  {
    public static readonly object TrueBox = true;
    public static readonly object FalseBox = false;

    public static object Box(bool value)
    {
      return value ? TrueBox : FalseBox;
    }
    
    public static object Box(bool? value)
    {
      if (value.HasValue)
      {
        return value.Value ? TrueBox : FalseBox;
      }

      return null;
    }
  }
}