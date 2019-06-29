// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 06 9:50 PM

namespace eagleboost.core.Logging
{
  public static class LoggerManager
  {
    public static ILoggerFacade GetLogger<T>()
    {
      var instance =  FileLogger<T>.Instance;
      return instance;
    }
  }
}