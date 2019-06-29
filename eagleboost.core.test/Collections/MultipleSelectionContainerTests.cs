// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 11:42 AM

namespace eagleboost.core.test.Collections
{
  using System;
  using eagleboost.core.Collections;
  using FluentAssertions;
  using NUnit.Framework;

  public class MultipleSelectionContainerTests
  {
    private static SelectionItem A = new SelectionItem("A");
    private static SelectionItem B = new SelectionItem("B");
    private static SelectionItem C = new SelectionItem("C");

    [Test]
    public void Task_01_Empty_01_Null_Access()
    {
      var c = new MultipleSelectionContainer<SelectionItem>();
      c[null].Should().BeFalse();
    }

    [Test]
    public void Task_01_Empty_02_Type_Mismatch_Access()
    {
      ISelectionContainer c = new MultipleSelectionContainer<SelectionItem>();
      Assert.Throws<ArgumentException>(() => c[new object()].Should().BeFalse());
    }

    [Test]
    public void Task_01_Empty_03_Item_Not_Exists()
    {
      var c = new MultipleSelectionContainer<SelectionItem>();
      c.SelectedItems.Count.Should().Be(0);
      c[A].Should().BeFalse();
    }

    [Test]
    public void Task_01_Empty_04_Unselect()
    {
      var c = new MultipleSelectionContainer<SelectionItem>();
      c.Unselect(A);
      c.SelectedItems.Count.Should().Be(0);
    }

    [Test]
    public void Task_02_Select_01_One()
    {
      var c = new MultipleSelectionContainer<SelectionItem>();
      c.Select(A);
      c.SelectedItems.Count.Should().Be(1);
      c.SelectedItems.Should().BeEquivalentTo(A);
      c[A].Should().BeTrue();
    }

    [Test]
    public void Task_02_Select_02_Multiple()
    {
      var c = new MultipleSelectionContainer<SelectionItem>();
      c.Select(B);
      c.Select(A);
      c.SelectedItems.Count.Should().Be(2);
      c.SelectedItems.Should().BeEquivalentTo(B, A);
      c[B].Should().BeTrue();
      c[A].Should().BeTrue();
    }

    [Test]
    public void Task_02_Select_03_Order()
    {
      var c = new MultipleSelectionContainer<SelectionItem>();
      c.Select(B);
      c.Select(A);
      c.Select(C);
      c.SelectedItems.Should().BeEquivalentTo(B, A, C);
    }

    [Test]
    public void Task_03_Unselect_01_One()
    {
      var c = new MultipleSelectionContainer<SelectionItem>();
      c.Select(A);
      c.Select(B);
      c.Unselect(B);
      c.SelectedItems.Count.Should().Be(1);
      c.SelectedItems.Should().BeEquivalentTo(A);
    }

    [Test]
    public void Task_03_Unselect_02_Multiple()
    {
      var c = new MultipleSelectionContainer<SelectionItem>();
      c.Select(A);
      c.Select(B);
      c.Select(C);
      c.Unselect(new []{A,C});
      c.SelectedItems.Should().BeEquivalentTo(B);
    }

    [Test]
    public void Task_04_Create_With_Initial_Selection()
    {
      var c = new MultipleSelectionContainer<SelectionItem>(new[] {B, A, C});
      c.SelectedItems.Should().BeEquivalentTo(B, A, C);
    }
  }
}