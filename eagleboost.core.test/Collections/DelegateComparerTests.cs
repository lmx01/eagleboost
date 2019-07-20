// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 18 9:25 PM

namespace eagleboost.core.test.Collections
{
  using System;
  using eagleboost.core.Collections;
  using FluentAssertions;
  using NUnit.Framework;

  /// <summary>
  /// DelegateComparerTests
  /// </summary>
  public class DelegateComparerTests
  {
    [Test]
    public void Task_01_Null()
    {
      Assert.Throws<ArgumentNullException>(() => new DelegateComparer<string>(null));
    }

    [Test]
    public void Task_01_Compare()
    {
      var c = new DelegateComparer<string>((x, y) => string.Compare(x, y, StringComparison.Ordinal));
      c.Compare(null, null).Should().Be(0);
      c.Compare(null, "A").Should().BeLessThan(0);
      c.Compare("A", null).Should().BeGreaterThan(0);
      c.Compare("A", "A").Should().Be(0);
      c.Compare("A", "B").Should().BeLessThan(0);
      c.Compare("B", "A").Should().BeGreaterThan(0);
    }
  }
}