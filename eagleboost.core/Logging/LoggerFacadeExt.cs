// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 06 11:11 PM

namespace eagleboost.core.Logging
{
  public static class LoggerFacadeExt
  {
    public static void Info(this ILoggerFacade log, string msg)
    {
      log.Log(msg, Category.Info);
    }

    public static void Info(this ILoggerFacade log, string format, params object[] parameter)
    {
      log.Log(string.Format(format, parameter), Category.Info);
    }

    public static void Warn(this ILoggerFacade log, string msg)
    {
      log.Log(msg, Category.Warn);
    }

    public static void Warn(this ILoggerFacade log, string format, params object[] parameter)
    {
      log.Log(string.Format(format, parameter), Category.Warn);
    }

    public static void Debug(this ILoggerFacade log, string msg)
    {
      log.Log(msg, Category.Debug);
    }

    public static void Debug(this ILoggerFacade log, string format, params object[] parameter)
    {
      log.Log(string.Format(format, parameter), Category.Debug);
    }

    public static void Error(this ILoggerFacade log, string msg)
    {
      log.Log(msg, Category.Exception);
    }

    public static void Error(this ILoggerFacade log, string format, params object[] parameter)
    {
      log.Log(string.Format(format, parameter), Category.Exception);
    }
  }
}