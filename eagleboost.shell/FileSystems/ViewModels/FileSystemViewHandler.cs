// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-01 9:16 PM

namespace eagleboost.shell.FileSystems.ViewModels
{
  using System.Threading;
  using eagleboost.presentation.Controls.Indicators;
  using eagleboost.presentation.Controls.TreeView;
  using eagleboost.presentation.Extensions;
  using eagleboost.shell.FileSystems.Contracts;
  using eagleboost.shell.FileSystems.Extensions;
  using Unity.Attributes;

  /// <summary>
  /// FileSystemViewHandler
  /// </summary>
  public class FileSystemViewHandler<TFile, TFolder>
    where TFile : class, IFile
    where TFolder : IFolder
  {
    #region Declarations
    private CancellationTokenSource _cts;
    #endregion Declarations

    #region Components
    [Dependency]
    public IFileSystemTreeViewModel TreeViewModel { get; set; }

    [Dependency]
    public IFileSystemCollectionViewModel<TFile, TFolder> ListViewModel { get; set; }

    public BusyStatusReceiver BusyStatusReceiver { get; set; }
    #endregion Components

    #region Init
    [InjectionMethod]
    public void Initialize()
    {
      TreeViewModel.PropertyChanged += HandleTreePropertyChanged;
      ListViewModel.FileSelected += HandleGridFileSelected;
    }
    #endregion Init

    #region Event Handlers
    private void HandleTreePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      var vm = (IFileSystemTreeViewModel)sender;
      if (e.PropertyName == "SelectedItem")
      {
        var containerNode = (ITreeNodeContainer)vm.SelectedItem;
        PopulateGrid(containerNode);
      }
    }

    private void HandleGridFileSelected(object sender, FileSelectedEventArgs<TFile> args)
    {
      var folder = args.File as IFolder;
      if (folder != null)
      {
        TreeViewModel.SelectAsync(folder);
      }
    }
    #endregion Event Handlers

    #region Private Methods
    private void CancelTask()
    {
      if (_cts != null)
      {
        _cts.Cancel();
        _cts = null;
      }
    }

    private CancellationToken CreateTaskToken()
    {
      CancelTask();

      _cts = new CancellationTokenSource();

      return _cts.Token;
    }

    private void PopulateGrid(ITreeNodeContainer folderNode)
    {
      if (folderNode != null)
      {
        var folder = (TFolder)folderNode.DataItem;
        BusyStatusReceiver.AutoReset("Loading...", () => ListViewModel.SetFolderAsync(folder, CreateTaskToken())).ConfigureAwait(true);
      }
    }
    #endregion Private Methods
  }
}