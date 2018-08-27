// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-25 10:21 PM

namespace eagleboost.core.test
{
  using eagleboost.core.Extensions;
  using NUnit.Framework;

  [TestFixture]
  public class FileExtTests
  {
    [Test]
    [TestCase(0, ExpectedResult = "0 bytes")]
    [TestCase(1, ExpectedResult = "1 bytes")]
    [TestCase(1000, ExpectedResult = "1000 bytes")]
    [TestCase(1024, ExpectedResult = "1 KB")]
    [TestCase(1500000, ExpectedResult = "1.4 MB")]
    [TestCase(1600000, ExpectedResult = "1.5 MB")]
    [TestCase(-1000, ExpectedResult = "-1000 bytes")]
    [TestCase(976, ExpectedResult = "976 bytes")]
    [TestCase(260226, ExpectedResult = "254.1 KB")]
    [TestCase(522310, ExpectedResult = "510 KB")]
    [TestCase(3000600, ExpectedResult = "2.9 MB")]
    [TestCase(11842813386, ExpectedResult = "11 GB")]
    [TestCase(1050, ExpectedResult = "1 KB")]
    [TestCase(10250, ExpectedResult = "10 KB")]
    [TestCase(10000000, ExpectedResult = "9.5 MB")]
    public string ToReadableSize(long size)
    {
      return size.ToReadableSize();
    }
  }
}