// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 06 10:51 PM

namespace eagleboost.core.Logging
{
  using System;
  using Prism.Logging;

  internal class ConsoleLogger : LoggerFacadeBase
  {
    public static ConsoleLogger Instance = new ConsoleLogger();

    protected override void Log(string message, Category category, Priority priority)
    {
      var prefix = GetPrefix(category);
      Console.WriteLine(prefix + message);
    }
  }
}