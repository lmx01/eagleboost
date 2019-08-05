namespace eagleboost.core.Logging
{
  /// <summary>
  /// CompositeLogger
  /// </summary>
  public class CompositeLogger<T> : LoggerFacadeBase
  {
    public static CompositeLogger<T> Instance = new CompositeLogger<T>();

    private CompositeLogger()
    {
    }
    
    #region Private Properties
    private ILoggerFacade LogTrace
    {
      get { return TraceLogger.Instance; }
    }

    private ILoggerFacade LogConsole
    {
      get { return ConsoleLogger.Instance; }
    }

    private ILoggerFacade LogFile
    {
      get { return FileLogger<T>.Instance; }
    }
    #endregion Private Properties
    
    protected override void Log(string message, Category category)
    {
      LogConsole.Log(message, category);
      LogFile.Log(message, category);
    }
  }
}