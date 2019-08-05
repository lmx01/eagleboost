namespace eagleboost.core.Logging
{
  using System.Diagnostics;

  /// <summary>
  /// TraceLogger
  /// </summary>
  public class TraceLogger : LoggerFacadeBase
  {
    public static TraceLogger Instance = new TraceLogger();

    private TraceLogger()
    {
    }
    
    protected override void Log(string message, Category category)
    {
      var prefix = GetPrefix(category);
      Trace.WriteLine(prefix + message);
    }
  }
}