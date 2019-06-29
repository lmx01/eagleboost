// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 06 11:23 PM

namespace eagleboost.core.Logging
{
  /// <summary>
  /// Category
  /// </summary>
  public enum Category
  {
    Debug,
    Exception,
    Warn,
    Info
  }

  /// <summary>
  /// ILoggerFacade
  /// </summary>
  public interface ILoggerFacade
  {
    void Log(string message, Category category);
  }
}