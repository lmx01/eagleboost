// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 06 9:50 PM

namespace eagleboost.core.Logging
{
  using System.Diagnostics;

  public static class LoggerManager
  {
    public static ILoggerFacade GetLogger<T>()
    {
      if (Debugger.IsAttached)
      {
        return CompositeLogger<T>.Instance;
      }
      
      return FileLogger<T>.Instance;
    }
  }
}