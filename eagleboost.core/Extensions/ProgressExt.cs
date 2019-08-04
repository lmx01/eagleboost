// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-23 12:02 AM

namespace eagleboost.core.Extensions
{
  using System;

  public static class ProgressExt
  {
    public static void TryReport<T>(this IProgress<T> progress, Func<T> statusFunc)
    {
      if (progress != null)
      {
        progress.Report(statusFunc());
      }
    }

    public static void TryReport(this IProgress<string> progress, string status)
    {
      if (progress != null)
      {
        progress.Report(status);
      }
    }

    public static void TryReport(this IProgress<string> progress, string format, params object[] values)
    {
      if (progress != null)
      {
        progress.Report(string.Format(format, values));
      }
    }
  }
}