// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 18 9:15 PM

namespace eagleboost.core.test.Collections
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using eagleboost.core.Collections;
  using eagleboost.core.ComponentModel;
  using FluentAssertions;
  using NUnit.Framework;
  using Assert = NUnit.Framework.Assert;

  /// <summary>
  /// CompositeComparerTests
  /// </summary>
  public class CompositeComparerTests
  {
    private static Item A = new Item("1", "A");
    private static Item A1 = new Item("2", "A");
    private static Item B = new Item("3", "B");

    [Test]
    public void Task_01_Null()
    {
      Assert.Throws<ArgumentNullException>(() => new CompositeComparer<Item>(null));
    }

    [Test]
    public void Task_02_Empty()
    {
      Assert.Throws<ArgumentException>(() => new CompositeComparer<Item>(new IComparer<Item>[0]));
    }

    private static object[] TestCases =
    {
      new object[] { (Item)null, (Item)null, null },
      new object[] { (Item)null, A, false},
      new object[] { A, (Item)null, true },
      new object[] { A, A, null},
      new object[] { A, B, false},
      new object[] { B, A, true},
      new object[] { A, A1, false},
      new object[] { A1, A, true},
    };

    [Test]
    [TestCaseSource("TestCases")]
    public void Task_03_Single(Item x, Item y, bool? result)
    {
      var dc = new DelegateComparer<Item>(CompareFirstName);
      var c = new CompositeComparer<Item>(new[] { dc });
      var v = c.Compare(x, y);
      if (v == 0)
      {
        result.Should().BeNull();
        return;
      }

      if (v < 0)
      {
        result.Should().BeFalse();
        return;
      }

      result.Should().BeTrue();
    }

    [Test]
    [TestCaseSource("TestCases")]
    public void Task_03_Single_Descending(Item x, Item y, bool? result)
    {
      var dc = new DelegateComparer<Item>(CompareFirstNameDescending);
      var c = new CompositeComparer<Item>(new[] { dc });
      var v = c.Compare(x, y);
      if (v == 0)
      {
        result.Should().BeNull();
        return;
      }

      if (v < 0)
      {
        result.Should().BeTrue();
        return;
      }

      result.Should().BeFalse();
    }

    [Test]
    public void Task_03_Single_Priority()
    {
      var pc = new PriorityComparer<Item>("Z");
      var dc = new DelegateComparer<Item>(CompareFirstName);
      var c = new CompositeComparer<Item>(new IComparer<Item>[] { pc, dc });
      var z = new Item("Z");
      c.Compare(A, z).Should().BeGreaterThan(0);
      c.Compare(z, A).Should().BeLessThan(0);
    }

    [Test]
    public void Task_04_Priority_Sort()
    {
      var pc = new PriorityComparer<Item>("Z", "Y");
      var dc = new DelegateComparer<Item>(CompareFirstName);
      var c = new CompositeComparer<Item>(new IComparer<Item>[] { pc, dc });
      var z = new Item("Z");
      var y = new Item("Y");
      var array = new[] { A, z, B, A1, y };
      Array.Sort(array, c);
      array.SequenceEqual(new[] { y, z, A, A1, B }).Should().BeTrue();
    }

    [Test]
    public void Task_05_Priority_Sort_Descending()
    {
      var pc = new PriorityComparer<Item>("Z", "Y");
      var dc = new DelegateComparer<Item>(CompareFirstNameDescending);
      var c = new CompositeComparer<Item>(new IComparer<Item>[] { pc, dc });
      var z = new Item("Z");
      var y = new Item("Y");
      var array = new[] { A, z, B, A1, y };
      Array.Sort(array, c);
      array.SequenceEqual(new[] { z, y, B, A1, A }).Should().BeTrue();
    }


    [Test]
    public void Task_06_MultiLevel()
    {
      var pc = new DelegateComparer<Item>(CompareFirstName);
      var dc = new DelegateComparer<Item>(CompareLastName);
      var c = new CompositeComparer<Item>(new IComparer<Item>[] { pc, dc });
      var a1 = new Item("a1", "Zhang", "A");
      var b1 = new Item("b1", "Zhang", "B");
      var c1 = new Item("c1", "Chen", "C");
      var array = new[] { a1, b1, c1 };
      Array.Sort(array, c);
      array.SequenceEqual(new[] { c1, a1, b1 }).Should().BeTrue();
    }

    private int CompareFirstNameDescending(Item x, Item y)
    {
      var r = CompareFirstName(x, y);
      return r * -1;
    }

    private int CompareFirstName(Item x, Item y)
    {
      if (x == null)
      {
        return y == null ? 0 : -1;
      }

      if (y == null)
      {
        return 1;
      }

      var r = string.Compare(x.FirstName, y.FirstName, StringComparison.Ordinal);
      if (r == 0)
      {
        return string.Compare(x.Id, y.Id, StringComparison.Ordinal);
      }

      return r;
    }

    private int CompareLastName(Item x, Item y)
    {
      var r = string.Compare(x.LastName, y.LastName, StringComparison.Ordinal);
      if (r == 0)
      {
        return string.Compare(x.Id, y.Id, StringComparison.Ordinal);
      }

      return r;
    }
  }

  public class Item : IItem
  {
    public Item(string firstName)
    {
      Id = FirstName = firstName;
    }

    public Item(string id, string firstName)
    {
      Id = id;
      FirstName = firstName;
    }

    public Item(string id, string firstName, string lastName)
    {
      Id = id;
      FirstName = firstName;
      LastName = lastName;
    }

    public string Id { get; private set; }

    public readonly string FirstName;

    public readonly string LastName;

    public override string ToString()
    {
      return Id == FirstName ? Id : Id + " - " + FirstName + ", " + LastName;

    }
  }

  public class PriorityComparer<T> : core.Collections.PriorityComparer<T> where T : Item
  {
    public PriorityComparer(params string[] names) : base(i => names.Contains(i.FirstName) ? 1 : 0)
    {
    }
  }
}