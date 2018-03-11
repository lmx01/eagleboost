using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace eagleboost.core.ComponentModel
{
  using eagleboost.core.Contracts;
  using eagleboost.core.Extensions;

  public class NotifyPropertyChangedBase : ObjectBase, INotifyPropertyChanging, INotifyPropertyChanged, IPropertyChangedNotifiable, INotifyPropertyChangeEventArgsProvider
  {
    #region Statics
    private static readonly Dictionary<string, PropertyChangedEventArgs> PropertyChangedEventArgs = new Dictionary<string, PropertyChangedEventArgs>();
    private static readonly Dictionary<string, PropertyChangingEventArgs> PropertyChangingEventArgs = new Dictionary<string, PropertyChangingEventArgs>();
    #endregion Statics

    public virtual event PropertyChangedEventHandler PropertyChanged;

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

    void IPropertyChangedNotifiable.OnPropertyChanging(string propertyName)
    {
      OnPropertyChanging(propertyName);
    }

    void IPropertyChangedNotifiable.OnPropertyChanged(string propertyName)
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
      return GetChangedArgs(member);
    }

    protected static PropertyChangingEventArgs GetChangingArgs<T>(Expression<Func<T, object>> expr)
    {
      var member = GetMember(expr);
      return GetChangingArgs(member);
    }

    protected static PropertyChangedEventArgs GetChangedArgs(string propertyName)
    {
      return PropertyChangedEventArgs.GetOrCreate(propertyName, p => new PropertyChangedEventArgs(p));
    }

    protected static PropertyChangingEventArgs GetChangingArgs(string propertyName)
    {
      return PropertyChangingEventArgs.GetOrCreate(propertyName, p => new PropertyChangingEventArgs(p));
    }

    public PropertyChangedEventArgs GetPropertyChangedArgs(string propertyName)
    {
      return GetChangedArgs(propertyName);
    }

    public PropertyChangingEventArgs GetPropertyChangingArgs(string propertyName)
    {
      return GetChangingArgs(propertyName);
    }
  }
}
