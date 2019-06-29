// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 11:27 AM

namespace eagleboost.core.test.Collections
{
  using System;
  using eagleboost.core.Collections;
  using FluentAssertions;
  using NUnit.Framework;

  public class RadioSelectionContainerTests
  {
    private static SelectionItem A = new SelectionItem("A");
    private static SelectionItem B = new SelectionItem("B");
    private static SelectionItem C = new SelectionItem("C");

    [Test]
    public void Task_01_Empty_Access()
    {
      var c = new RadioSelectionContainer<SelectionItem>();
      Assert.Throws<InvalidOperationException>(() => c[A].Should().BeFalse());
    }

    [Test]
    public void Task_02_Unselect()
    {
      var c = new RadioSelectionContainer<SelectionItem>();
      c.Select(A);
      c.Unselect(A);
      c.SelectedItems.Should().BeEquivalentTo(A);
    }
  }
}