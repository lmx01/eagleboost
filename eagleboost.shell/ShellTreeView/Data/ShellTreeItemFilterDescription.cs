namespace eagleboost.shell.ShellTreeView.Data
{
  using eagleboost.core.ComponentModel;
  using eagleboost.core.Extensions;
  using eagleboost.presentation.Collections;
  using eagleboost.shell.ShellTreeView.Contracts;

  public class ShellTreeItemFilterDescription : NotifyPropertyChangedBase, IFilterDescription
  {
    private string _value;
    
    public ShellTreeItemFilterDescription(string memberName)
    {
      MemberName = memberName;
    }
    
    public string MemberName { get; private set; }

    public string Value
    {
      get { return _value; }
      set { SetValue(ref _value, value); }
    }

    public bool Match(object obj)
    {
      if (_value == null)
      {
        return true;
      }

      var shellItem = (IShellTreeItem) obj;
      if (!shellItem.IsExpanded)
      {
        return shellItem.Name.ContainsNoCase(Value);
      }
      else
      {
        if (MatchShellItem(shellItem, Value))
        {
          return true;
        }
      }

      return shellItem.Name.ContainsNoCase(Value);
    }

    private bool MatchShellItem(IShellTreeItem item,string value)
    {
      foreach (var child in item.Children)
      {
        if (child.IsExpanded)
        {
          var m = MatchShellItem(child, value);
          if (m)
          {
            return true;
          }
        }
        else if (child.Name.ContainsNoCase(value))
        {
          return true;
        }
      }

      return false;
    }
  }
}