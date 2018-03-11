namespace eagleboost.shell.ShellTreeView.ViewModels
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Linq;
  using System.Windows.Data;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.Extensions;
  using eagleboost.shell.ShellTreeView.Contracts;
  using eagleboost.shell.ShellTreeView.Data;
  using Microsoft.WindowsAPICodePack.Shell;

  public partial class ShellTreeViewModel : NotifyPropertyChangedBase, IShellItemsOperation
  {
    private static readonly IKnownFolder DesktopKnownFolder = KnownFolders.Desktop;
    private readonly ObservableCollection<IShellTreeItem> _shellItems = new ObservableCollection<IShellTreeItem>();
    private readonly ListCollectionView _shellItemsView;
    private readonly HashSet<ShellTreeItem> _expandedItems = new HashSet<ShellTreeItem>();

    public ShellTreeViewModel()
    {
      _shellItemsView = new ListCollectionView(_shellItems) {Filter = o => Filter((ShellTreeItem) o)};
    }

    public ICollectionView ShellItemsView
    {
      get { return _shellItemsView; }
    }

    public ObservableCollection<IShellTreeItem> ShellItems
    {
      get { return _shellItems; }
    }

    public ShellTreeViewOptions Options { get; private set; }

    public void Initialize(ShellTreeViewOptions options)
    {
      Options = options;
      foreach (var item in CreateChildItems(DesktopKnownFolder, null))
      {
        _shellItems.Add(item);
      }

      Options.PropertyChanged += HandleOptionsChanged;
    }

    private bool Filter(ShellObject shellObj)
    {
      if (!Options.ShowFiles)
      {
        if (shellObj is ShellFile)
        {
          return false;
        }
      }
      else
      {
        if (Options.FileFilters.Any())
        {
          var shellFile = shellObj as ShellFile;
          if (shellFile != null)
          {
            foreach (var ext in Options.FileFilters)
            {
              if (!shellFile.Path.EndsWith(ext))
              {
                return false;
              }
            }
          }
        }
      }

      var filterText = Options.FilterText;
      if (!filterText.IsNullOrEmpty())
      {
        return shellObj.Name.ContainsNoCase(filterText);
      }

      return true;
    }

    public IEnumerable<IShellTreeItem> CreateChildItems(IEnumerable<ShellObject> shellObjects, ShellTreeContainerItem parent)
    {
      foreach (var shellObject in shellObjects)
      {
        var item = shellObject is ShellContainer
          ? (IShellTreeItem) new ShellTreeContainerItem(shellObject, parent, this)
          : new ShellTreeObjectItem(shellObject, parent, this);
        item.PropertyChanged += HandleItemPropertyChanged;

        yield return item;
      }
    }

    public bool Filter(IShellTreeItem item)
    {
      return Filter(item.ShellObject);
    }

    private void HandleItemPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (ShellTreeItem.IsExpandedArgs.Match(e))
      {
        var item = (ShellTreeItem) sender;
        if (item.IsExpanded)
        {
          _expandedItems.Add(item);
        }
        else
        {
          _expandedItems.Remove(item);
        }
      }
    }

    private IShellTreeItem _myPcItem;
    private IShellTreeItem MyPcItem
    {
      get
      {
        if (_myPcItem == null)
        {
          foreach (var item in _shellItems)
          {
            if (item.ShellObject.ParsingName == "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}")
            {
              _myPcItem = item;
                  break;
            }
          }
        }

        return _myPcItem;
      }
    }
    
    public void SetPath(string path)
    {
      if (path.IsNullOrEmpty())
      {
        return;
      }

      var parts = path.Split('\\');
      var partsQueue = new Queue<string>(parts);
      var pathPart= string.Format("({0})",partsQueue.Dequeue());
      if (partsQueue.Count > 0)
      {
        var rootItem = FindRootItem(pathPart);
        if (rootItem != null)
        {
          if (partsQueue.Count > 0)
          {
            rootItem.IsExpanded = true;
            pathPart = partsQueue.Dequeue();
            LocatePath(rootItem, pathPart, partsQueue);
          }
          else
          {
            rootItem.IsSelected = true;
          }
        }
        else
        {
          MyPcItem.IsExpanded = true;
          LocatePath(MyPcItem, pathPart, partsQueue);
        }  
      }
      else
      {
        MyPcItem.IsSelected = true;
      }
    }

    private IShellTreeItem FindRootItem(string pathPart)
    {
      foreach (var item in _shellItems)
      {
        var folder = item.ShellObject as FileSystemKnownFolder;
        if (folder != null)
        {
          var folderName = folder.Name;
          if (folderName.EqualsNoCase(pathPart) || folderName.ContainsNoCase(pathPart))
          {
            return item;
          }
        }
      }

      return null;
    }
    
    private bool LocatePath(IShellTreeItem parent, string pathPart, Queue<string> partsQueue)
    {
      parent.IsExpanded = true;
      foreach (var item in parent.Children)
      {
        var folder = item.ShellObject as FileSystemKnownFolder;
        if (folder != null)
        {
          var folderName = folder.Name;
          if (folderName.EqualsNoCase(pathPart) || folderName.ContainsNoCase(pathPart))
          {
            if (partsQueue.Count == 0)
            {
              item.IsSelected = true;
              return true;
            }
            pathPart = partsQueue.Dequeue();
            return LocatePath(item, pathPart, partsQueue);
          }
        }
      }

      return false;
    }
  }
}
