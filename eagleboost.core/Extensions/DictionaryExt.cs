// Author : Shuo Zhang
// 
// Creation :2018-03-02 23:22

namespace eagleboost.core.Extensions
{
  using System;
  using System.Collections.Generic;

  public static class DictionaryExt
  {
    public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
      Func<TKey, TValue> func)
    {
      TValue value;
      if (!dict.TryGetValue(key, out value))
      {
        dict[key] = value = func(key);
      }

      return value;
    }

    public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) where TValue : new()
    {
      TValue value;
      if (!dict.TryGetValue(key, out value))
      {
        dict[key] = value = new TValue();
      }

      return value;
    }
  }
}