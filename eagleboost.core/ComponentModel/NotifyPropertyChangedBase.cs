namespace eagleboost.core.ComponentModel
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq.Expressions;
  using System.Runtime.CompilerServices;
  using eagleboost.core.Contracts;

  public class NotifyPropertyChangedBase : ObjectBase, INotifyPropertyChanging, INotifyPropertyChanged, IExternalPropertyChangeNotify
  {
    public virtual event PropertyChangedEventHandler PropertyChanged;

    protected virtual void NotifyPropertyChanged(PropertyChangedEventArgs args)
    {
      var handler = PropertyChanged;
      if (handler != null)
      {
        handler(this, args);
      }
    }

    protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
    {
      var handler = PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    public virtual event PropertyChangingEventHandler PropertyChanging;

    protected virtual void NotifyPropertyChanging([CallerMemberName] string propertyName = null)
    {
      var handler = PropertyChanging;
      if (handler != null)
      {
        handler(this, new PropertyChangingEventArgs(propertyName));
      }
    }

    protected void NotifyPropertyReset<T>(ref T field, [CallerMemberName] string property = null) where T : class
    {
      NotifyPropertyChanging(property);
      field = null;
      NotifyPropertyChanged(property);
      OnPropertyChanged(property);
    }

    protected bool SetValue<T>(ref T field, T value, [CallerMemberName] string property = null)
    {
      if (!EqualityComparer<T>.Default.Equals(field, value))
      {
        NotifyPropertyChanging(property);
        field = value;
        NotifyPropertyChanged(property);
        OnPropertyChanged(property);
        return true;
      }

      return false;
    }

    protected void Force<T>(ref T field, T value, [CallerMemberName] string property = null)
    {
      NotifyPropertyChanging(property);
      field = value;
      NotifyPropertyChanged(property);
      OnPropertyChanged(property);
    }

    void IExternalPropertyChangeNotify.OnPropertyChanging(string propertyName)
    {
      OnPropertyChanging(propertyName);
    }

    void IExternalPropertyChangeNotify.OnPropertyChanged(string propertyName)
    {
      OnPropertyChanged(propertyName);
    }

    protected virtual void OnPropertyChanging(string propertyName)
    {
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
    }

    protected static PropertyChangedEventArgs GetChangedArgs<T>(Expression<Func<T, object>> expr)
    {
      var member = GetMember(expr);
      return new PropertyChangedEventArgs(member);
    }

    protected static PropertyChangingEventArgs GetChangingArgs<T>(Expression<Func<T, object>> expr)
    {
      var member = GetMember(expr);
      return new PropertyChangingEventArgs(member);
    }
  }
}
