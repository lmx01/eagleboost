using System;
using System.IO;
using System.Reflection;

namespace eagleboost.core.Utils
{
  public static class EmbeddedResourceLoader
  {
    public static string GetTextFile(Assembly assembly, string resourceName)
    {
      try
      {
        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
          using (var reader = new StreamReader(stream))
          {
            return reader.ReadToEnd();
          }
        }
      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}