// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-25 10:15 PM

namespace eagleboost.core.Extensions
{
  using System;

  public static class FileExt
  {
    private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB" };

    public static string ToReadableSize(this long byteSize)
    {
      string sizeStr;
      string sizeUnit;
      return byteSize.ToReadableSize(out sizeStr, out sizeUnit);
    }

    public static string ToReadableSize(this long byteSize, out string sizeStr, out string sizeUnit)
    {
      var order = 0;
      double size = byteSize;
      while (size >= 1024 && order < SizeSuffixes.Length - 1)
      {
        order++;
        size = size / 1024;
      }

      if (Math.Abs(size - Math.Floor(size)) < 0.1)
      {
        size = Math.Floor(size);
      }

      sizeStr = string.Format("{0:0.#}", size);
      sizeUnit = SizeSuffixes[order];
      return sizeStr + " " + sizeUnit;
    }
  }
}