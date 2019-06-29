// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 29 10:50 AM

namespace eagleboost.core.test.Collections
{
  using System;
  using eagleboost.core.Collections;
  using FluentAssertions;
  using NUnit.Framework;
  using Assert = NUnit.Framework.Assert;

  public class SelectionItem : IComparable<SelectionItem>, IComparable
  {
    public SelectionItem(string name)
    {
      Name = name;
    }

    public string Name { get; set; }

    public override string ToString()
    {
      return Name;
    }

    public int CompareTo(object obj)
    {
      return CompareTo(obj as SelectionItem);
    }

    public int CompareTo(SelectionItem other)
    {
      if (ReferenceEquals(this, other)) return 0;
      if (ReferenceEquals(null, other)) return 1;
      return string.Compare(Name, other.Name, StringComparison.Ordinal);
    }
  }

  public class SingleSelectionContainerTests
  {
    private static SelectionItem A = new SelectionItem("A");
    private static SelectionItem B = new SelectionItem("B");
    private static SelectionItem C = new SelectionItem("C");

    [Test]
    public void Task_01_Empty_01_Null_Access()
    {
      var c = new SingleSelectionContainer<SelectionItem>();
      c[null].Should().BeFalse();
    }

    [Test]
    public void Task_01_Empty_02_Type_Mismatch_Access()
    {
      ISelectionContainer c = new SingleSelectionContainer<SelectionItem>();
      Assert.Throws<ArgumentException>(() => c[new object()].Should().BeFalse());
    }

    [Test]
    public void Task_01_Empty_03_Item_Not_Exists()
    {
      var c = new SingleSelectionContainer<SelectionItem>();
      c.SelectedItems.Count.Should().Be(0);
      c[A].Should().BeFalse();
    }

    [Test]
    public void Task_01_Empty_04_Unselect()
    {
      var c = new SingleSelectionContainer<SelectionItem>();
      c.Unselect(A);
      c.SelectedItems.Count.Should().Be(0);
    }

    [Test]
    public void Task_02_Select_01_One()
    {
      var c = new SingleSelectionContainer<SelectionItem>();
      c.Select(A);
      c.SelectedItems.Count.Should().Be(1);
      c.SelectedItems.Should().BeEquivalentTo(A);
      c[A].Should().BeTrue();
    }

    [Test]
    public void Task_02_Select_02_Multiple()
    {
      var c = new SingleSelectionContainer<SelectionItem>();
      c.Select(A);
      c.Select(B);
      c.SelectedItems.Count.Should().Be(1);
      c.SelectedItems.Should().BeEquivalentTo(B);
      c[B].Should().BeTrue();
      c[A].Should().BeFalse();
    }

    [Test]
    public void Task_03_Unselect_01_One()
    {
      var c = new SingleSelectionContainer<SelectionItem>();
      c.Select(A);
      c.Select(B);
      c.Unselect(B);
      c.SelectedItems.Count.Should().Be(0);
    }

    [Test]
    public void Task_03_Unselect_02_Multiple()
    {
      var c = new SingleSelectionContainer<SelectionItem>();
      c.Select(A);
      c.Select(B);
      c.Select(C);
      c.Unselect(C);
      c.SelectedItems.Count.Should().Be(0);
      c.Unselect(B);
      c.SelectedItems.Count.Should().Be(0);
      c.Unselect(A);
      c.SelectedItems.Count.Should().Be(0);
    }

    [Test]
    public void Task_04_Create_With_Initial_Selection()
    {
      var c = new SingleSelectionContainer<SelectionItem>(A);
      c.SelectedItems.Should().BeEquivalentTo(A);
    }
  }
}