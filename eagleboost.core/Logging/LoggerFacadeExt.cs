// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 06 11:11 PM

namespace eagleboost.core.Logging
{
  using Prism.Logging;

  public static class LoggerFacadeExt
  {
    public static void Info(this ILoggerFacade log, string msg)
    {
      log.Log(msg, Category.Info, Priority.None);
    }

    public static void Info(this ILoggerFacade log, string format, params object[] parameter)
    {
      log.Log(string.Format(format, parameter), Category.Info, Priority.None);
    }

    public static void Warn(this ILoggerFacade log, string msg)
    {
      log.Log(msg, Category.Warn, Priority.None);
    }

    public static void Warn(this ILoggerFacade log, string format, params object[] parameter)
    {
      log.Log(string.Format(format, parameter), Category.Warn, Priority.None);
    }

    public static void Debug(this ILoggerFacade log, string msg)
    {
      log.Log(msg, Category.Debug, Priority.None);
    }

    public static void Debug(this ILoggerFacade log, string format, params object[] parameter)
    {
      log.Log(string.Format(format, parameter), Category.Debug, Priority.None);
    }

    public static void Error(this ILoggerFacade log, string msg)
    {
      log.Log(msg, Category.Exception, Priority.None);
    }

    public static void Error(this ILoggerFacade log, string format, params object[] parameter)
    {
      log.Log(string.Format(format, parameter), Category.Exception, Priority.None);
    }
  }
}