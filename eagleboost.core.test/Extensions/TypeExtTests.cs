// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 07 12:11 AM

namespace eagleboost.core.test.Extensions
{
  using eagleboost.core.Extensions;
  using FluentAssertions;
  using NUnit.Framework;

  public class TypeExtTests
  {
    [Test]
    [TestCase("eagleboost.core.test.Extensions.TypeExtTests", "e.c.t.E.TypeExtTests")]
    [TestCase("Eagleboost.Core.Test.TestModule.TypeExtTests", "E.C.T.TM.TypeExtTests")]
    [TestCase(null, null)]
    [TestCase("", null)]
    public void GetTypeName(string name, string result)
    {
      TypeExt.GetName(name).Should().Be(result);
    }
  }
}