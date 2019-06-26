// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 25 8:45 PM

namespace eagleboost.shell.test.FileSystems
{
  using System.Linq;
  using eagleboost.core.Collections;
  using eagleboost.shell.FileSystems.Contracts;
  using eagleboost.shell.FileSystems.ViewModels;
  using FluentAssertions;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using NSubstitute;

  [TestClass]
  public class FileSystemHistoryTests
  {
    [TestMethod]
    public void Task_01_Creation()
    {
      var h = new FileSystemHistory();
      h.Current.Should().BeNull();
      h.HasBackwardHistory.Should().BeFalse();
      h.HasForwardHistory.Should().BeFalse();
    }

    [TestMethod]
    public void Task_02_Add_One()
    {
      var h = new FileSystemHistory();
      var entry = CreateEntry("Folder");
      h.Add(entry);
      h.Current.Should().Be(entry);
      h.HasBackwardHistory.Should().BeFalse();
      h.HasForwardHistory.Should().BeFalse();
    }

    [TestMethod]
    public void Task_03_Add_More()
    {
      var h = new FileSystemHistory();
      var entry = CreateEntry("Folder");
      h.Add(entry);
      var entry2 = CreateEntry("Folder");
      h.Add(entry2);
      h.Current.Should().Be(entry2);
      h.HasBackwardHistory.Should().BeTrue();
      h.HasForwardHistory.Should().BeFalse();
      var backView = h.BackwardHistoryView.GetView<FileSystemHistoryOperations>();
      backView.Count.Should().Be(1);
      backView[0].Should().Be(entry);
    }

    [TestMethod]
    public void Task_04_Navigate_Back()
    {
      var h = new FileSystemHistory();
      var entry1 = CreateEntry("Folder1");
      h.Add(entry1);
      var entry2 = CreateEntry("Folder2");
      h.Add(entry2);
      h.NavigateBack();
      h.Current.Should().Be(entry1);
      h.HasBackwardHistory.Should().BeFalse();
      h.HasForwardHistory.Should().BeTrue();
      var forwardView = h.ForwardHistoryView.GetView<FileSystemHistoryOperations>();
      forwardView.Count.Should().Be(1);
      forwardView[0].Should().Be(entry2);
    }

    [TestMethod]
    public void Task_05_Navigate_Forward()
    {
      var h = new FileSystemHistory();
      var entry1 = CreateEntry("Folder1");
      h.Add(entry1);
      var entry2 = CreateEntry("Folder2");
      h.Add(entry2);
      h.NavigateBack();
      h.NavigateForward();
      h.Current.Should().Be(entry2);
      h.HasBackwardHistory.Should().BeTrue();
      h.HasForwardHistory.Should().BeFalse();
      var backView = h.BackwardHistoryView.GetView<FileSystemHistoryOperations>();
      backView.Count.Should().Be(1);
      backView[0].Should().Be(entry1);
    }

    [TestMethod]
    public void Task_06_Navigate_Random()
    {
      var h = new FileSystemHistory();
      var entry1 = CreateEntry("Folder1");
      h.Add(entry1);
      var entry2 = CreateEntry("Folder2");
      h.Add(entry2);
      var entry3 = CreateEntry("Folder3");
      h.Add(entry3);
      entry2.RaiseNavigate();
      h.Current.Should().Be(entry2);
      h.HasBackwardHistory.Should().BeTrue();
      h.HasForwardHistory.Should().BeTrue();
      var backView = h.BackwardHistoryView.GetView<FileSystemHistoryOperations>();
      backView.Count.Should().Be(1);
      backView[0].Should().Be(entry1);
      var forwardView = h.ForwardHistoryView.GetView<FileSystemHistoryOperations>();
      forwardView.Count.Should().Be(1);
      forwardView[0].Should().Be(entry3);
    }

    [TestMethod]
    public void Task_07_Add_When_Current_Is_In_Middle()
    {
      var h = new FileSystemHistory();
      var entry1 = CreateEntry("Folder1");
      h.Add(entry1);
      var entry2 = CreateEntry("Folder2");
      h.Add(entry2);
      var entry3 = CreateEntry("Folder3");
      h.Add(entry3);
      entry2.RaiseNavigate();
      var entry4 = CreateEntry("Folder4");
      h.Add(entry4);
      h.HistoryEntries.Contains(entry3).Should().BeFalse();
      var backView = h.BackwardHistoryView.GetView<FileSystemHistoryOperations>();
      backView.Count.Should().Be(2);
      backView[0].Should().Be(entry1);
      backView[1].Should().Be(entry2);
      h.HasForwardHistory.Should().BeFalse();
    }

    private IFolder CreateFolder(string name)
    {
      var folder = Substitute.For<IFolder>();
      folder.DisplayName.Returns(name);
      return folder;
    }

    private FileSystemHistoryOperations CreateEntry(string name)
    {
      return new FileSystemHistoryOperations(CreateFolder(name), e => { });
    }
  }
}