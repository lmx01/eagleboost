// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 18 10:29 PM

namespace eagleboost.presentation.MarkupExtensions
{
using System;
using System.Windows;
using System.Windows.Markup;

  public abstract class MarkupExtensionBase : MarkupExtension
  {
    protected virtual bool TryGetTargetItems(IServiceProvider provider, out DependencyObject target, out DependencyProperty dp)
    {
      target = null;
      dp = null;
      if (provider == null) return false;

      //create a binding and assign it to the target
      IProvideValueTarget service = (IProvideValueTarget)provider.GetService(typeof(IProvideValueTarget));
      if (service == null) return false;

      //we need dependency objects / properties
      target = service.TargetObject as DependencyObject;
      dp = service.TargetProperty as DependencyProperty;
      return target != null && dp != null;
    }

    protected virtual bool TryGetTargetItems<T>(IServiceProvider provider, out T target, out DependencyProperty dp) where T : DependencyObject
    {
      DependencyObject obj;
      DependencyProperty property;
      if (TryGetTargetItems(provider, out obj, out property))
      {
        target = obj as T;
        dp = property;
        return true;
      }

      target = null;
      dp = null;
      return false;
    }

    protected bool IsTemplated(IServiceProvider provider)
    {
      if (provider == null) return false;

      //create a binding and assign it to the target
      IProvideValueTarget service = (IProvideValueTarget)provider.GetService(typeof(IProvideValueTarget));
      if (service == null) return false;

      //we need dependency objects / properties
      var targetObj = service.TargetObject;
      if (targetObj != null && targetObj.GetType().Name == "SharedDp")
      {
        return true;
      }

      return false;
    }
  }
}