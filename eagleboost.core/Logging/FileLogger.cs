// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 06 11:29 PM

namespace eagleboost.core.Logging
{
  using System.Reflection;
  using System.Text;
  using eagleboost.core.Extensions;
  using log4net;
  using Prism.Logging;

  public class FileLogger<T> : LoggerFacadeBase
  {
    public static FileLogger<T> Instance
    {
      get { return Nested.NestedInstance; }
    }

    private class Nested
    {
      static Nested()
      {
      }

      internal static readonly FileLogger<T> NestedInstance = new FileLogger<T>();
    }

    private FileLogger()
    {
      var name = typeof(T).GetName();
      _log = LogManager.GetLogger(name);
    }

    private readonly ILog _log;

    protected override void Log(string message, Category category, Priority priority)
    {
      switch (category)
      {
        case Category.Debug:
          Debug(message);
          return;

        case Category.Exception:
          Error(message);
          return;

        case Category.Warn:
          Warn(message);
          return;
      }

      Info(message);
    }

    private void Debug(string msg)
    {
      if (_log.IsDebugEnabled)
      {
        _log.Debug(msg);
      }
    }

    private void Error(string msg)
    {
      if (_log.IsErrorEnabled)
      {
        _log.Error(msg);
      }
    }

    private void Warn(string msg)
    {
      if (_log.IsWarnEnabled)
      {
        _log.Warn(msg);
      }
    }

    private void Info(string msg)
    {
      if (_log.IsInfoEnabled)
      {
        _log.Info(msg);
      }
    }
  }
}