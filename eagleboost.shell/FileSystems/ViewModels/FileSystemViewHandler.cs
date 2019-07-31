// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-09-01 9:16 PM

namespace eagleboost.shell.FileSystems.ViewModels
{
  using System.ComponentModel;
  using System.Diagnostics;
  using System.Linq;
  using System.Threading;
  using eagleboost.core.Extensions;
  using eagleboost.core.Threading;
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
    private readonly CancellableTaskHandler _loadFileInfoHandler = new CancellableTaskHandler();
    #endregion Declarations

    #region Components
    [Dependency]
    public IFileSystemTreeViewModel TreeViewModel { get; set; }

    [Dependency]
    public IFileSystemCollectionViewModel<TFile, TFolder> ListViewModel { get; set; }

    [Dependency]
    public IFileSystemFileInfoViewModel<TFile, TFolder> FileInfoViewModel { get; set; }

    public BusyStatusReceiver BusyStatusReceiver { get; set; }
    #endregion Components

    #region Init
    [InjectionMethod]
    public void Initialize()
    {
      TreeViewModel.PropertyChanged += HandleTreePropertyChanged;
      ListViewModel.FileSelected += HandleGridFileSelected;
      if (FileInfoViewModel != null)
      {
        ListViewModel.PropertyChanged += HandleGridViewModelPropertyChanged;
      }
    }
    #endregion Init

    #region Private Properties
    private double Timeout
    {
      get
      {
        if (Debugger.IsAttached)
        {
          return 1000 * 600;
        }

        return 1000 * 5;
      }
    }
    #endregion Private Properties

    #region Event Handlers
    private void HandleTreePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      var vm = (IFileSystemTreeViewModel)sender;
      if (e.PropertyName == "SelectedItem")
      {
        var containerNode = vm.SelectedItem as ITreeNodeContainer;
        PopulateGrid(containerNode);
      }
    }

    private async void HandleGridFileSelected(object sender, FileSelectedEventArgs<TFile> args)
    {
      var folder = args.File as IFolder;
      if (folder != null)
      {
        ////The selected file may be some adhoc files that don't belong to any folder
        ////so we don't want the grid to be populated by HandleTreePropertyChanged.
        var tree = TreeViewModel;
        tree.PropertyChanged -= HandleTreePropertyChanged;
        await tree.SelectAsync(folder).ConfigureAwait(true);
        tree.PropertyChanged += HandleTreePropertyChanged;

        var list = ListViewModel;
        var node = list.CurrentFolderNode.Children.FirstOrDefault(i => Equals(i.DataItem, folder)) as ITreeNodeContainer;
        if (node != null)
        {
          PopulateGrid(node);
        }
      }
    }

    private void HandleGridViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      var vm = (IFileSystemCollectionViewModel<TFile, TFolder>)sender;
      if (e.PropertyName == "SelectedItem")
      {
        var file = vm.SelectedItem;
        if (file != null && file.IsNot<IFolder>())
        {
          var br = BusyStatusReceiver;
          br.AutoReset("Loading file info...", () => _loadFileInfoHandler.ExecuteAsync(ct => FileInfoViewModel.LoadFileInfoAsync(file, ct,br), Timeout))
            .ConfigureAwait(true);
        }
      }
    }
    #endregion Event Handlers

    #region Private Methods
    private void PopulateGrid(ITreeNodeContainer folderNode)
    {
      if (folderNode != null)
      {
        var cts = new CancellationTokenSource();
        ListViewModel.SetFolderAsync(folderNode, cts.Token, BusyStatusReceiver);
      }
    }
    #endregion Private Methods
  }
}