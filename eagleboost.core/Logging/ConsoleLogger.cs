// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 06 10:51 PM

namespace eagleboost.core.Logging
{
  using System;

  /// <summary>
  /// ConsoleLogger
  /// </summary>
  internal class ConsoleLogger : LoggerFacadeBase
  {
    public static ConsoleLogger Instance = new ConsoleLogger();

    private ConsoleLogger()
    {
    }
    
    protected override void Log(string message, Category category)
    {
      var prefix = GetPrefix(category);
      Console.WriteLine(prefix + message);
    }
  }
}