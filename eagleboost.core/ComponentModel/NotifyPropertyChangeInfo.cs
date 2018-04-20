// Author : Shuo Zhang
// 
// Creation :2018-03-16 14:24

namespace eagleboost.core.ComponentModel
{
  using System.ComponentModel;
  using System.Reflection;

  public static class NotifyPropertyChangeInfo
  {
    #region Statics
    public static readonly EventInfo PropertyChangingEventInfo = typeof(INotifyPropertyChanging).GetEvent("PropertyChanging");
    public static readonly MethodInfo AddPropertyChangingMethodInfo = PropertyChangingEventInfo.GetAddMethod();
    public static readonly string AddPropertyChangingMethodName = AddPropertyChangingMethodInfo.Name;
    public static readonly MethodInfo RemovePropertyChangingMethodInfo = PropertyChangingEventInfo.GetRemoveMethod();
    public static readonly string RemovePropertyChangingMethodName = RemovePropertyChangingMethodInfo.Name;

    public static readonly EventInfo PropertyChangedEventInfo = typeof(INotifyPropertyChanged).GetEvent("PropertyChanged");
    public static readonly MethodInfo AddPropertyChangedMethodInfo = PropertyChangedEventInfo.GetAddMethod();
    public static readonly string AddPropertyChangedMethodName = AddPropertyChangedMethodInfo.Name;
    public static readonly MethodInfo RemovePropertyChangedMethodInfo = PropertyChangedEventInfo.GetRemoveMethod();
    public static readonly string RemovePropertyChangedMethodName = RemovePropertyChangedMethodInfo.Name;

    public static readonly MethodInfo NotifyPropertyChangedMethodInfo = typeof(NotifyPropertyChangedBase).GetMethod("NotifyPropertyChanged", BindingFlags.NonPublic|BindingFlags.Instance);
    public static readonly string NotifyPropertyChangedMethodName = NotifyPropertyChangedMethodInfo.Name;
    public static readonly MethodInfo NotifyPropertyChangingMethodInfo = typeof(NotifyPropertyChangedBase).GetMethod("NotifyPropertyChanging", BindingFlags.NonPublic | BindingFlags.Instance);
    public static readonly string NotifyPropertyChangingMethodName = NotifyPropertyChangingMethodInfo.Name;
    #endregion Statics
  }
}