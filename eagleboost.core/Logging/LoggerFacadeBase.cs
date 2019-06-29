// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 06 10:56 PM

namespace eagleboost.core.Logging
{
  public abstract class LoggerFacadeBase : ILoggerFacade
  {
    private const string Debug = "[Debug] ";
    private const string Exception = "[Error] ";
    private const string Info = "[Info] ";
    private const string Warn = "[Warn] ";

    protected static string GetDefaultPrefix(Category category)
    {
      switch (category)
      {
        case Category.Debug:
          return Debug;

        case Category.Exception:
          return Exception;

        case Category.Warn:
          return Warn;

        default:
          return Info;
      }
    }

    void ILoggerFacade.Log(string message, Category category)
    {
      Log(message, category);
    }

    protected virtual string GetPrefix(Category category)
    {
      return GetDefaultPrefix(category);
    }

    protected abstract void Log(string message, Category category);
  }
}