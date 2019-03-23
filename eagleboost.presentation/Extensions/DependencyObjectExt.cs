namespace eagleboost.presentation.Extensions
{
  using System;
  using System.Collections.Generic;
  using System.Windows;
  using System.Windows.Data;
  using System.Windows.Media;

  public static class DependencyObjectExt
  {
    /// <summary>
    /// Set valut to the property is the property is not set yet
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="property"></param>
    /// <param name="value"></param>
    public static void SetValueIfDefault(this DependencyObject obj, DependencyProperty property, object value)
    {
      if (value != null && DependencyPropertyHelper.GetValueSource(obj, property).BaseValueSource == BaseValueSource.Default)
      {
        obj.SetValue(property, value);
      }
    }

    /// <summary>
    /// Find a sequence of children of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static IEnumerable<T> FindChildren<T>(this DependencyObject parent) where T : DependencyObject
    {
      return FindChildren<T>(parent, null);
    }

    /// <summary>
    /// Find a sequence of children of type T and apply filter if applicable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public static IEnumerable<T> FindChildren<T>(this DependencyObject parent, Predicate<T> filter) where T : DependencyObject
    {
      var count = VisualTreeHelper.GetChildrenCount(parent);
      for (var i = 0; i < count; i++)
      {
        var childItem = VisualTreeHelper.GetChild(parent, i);

        // If the child is not of the request child type child
        var child = childItem as T;
        if (child == null)
        {
          // recursively drill down the tree
          foreach (var findChild in FindChildren(childItem, filter))
          {
            yield return findChild;
          }
        }
        else if (filter == null || filter(child))
        {
          yield return child;
        }
      }
    }

    public static T FindParent<T>(this DependencyObject d) where T : DependencyObject
    {
      if (d == null)
      {
        throw new ArgumentNullException("d");
      }

      DependencyObject dpo = VisualTreeHelper.GetParent(d);

      while (dpo != null && !(dpo is T))
      {
        DependencyObject parent = VisualTreeHelper.GetParent(dpo);
        if (parent != null)
        {
          dpo = parent;
        }
        else
        {
          if (!(dpo is FrameworkElement))
          {
            break;
          }
          dpo = (dpo as FrameworkElement).Parent;
        }
      }

      return (T)dpo;
    }

    public static DependencyObject FindParent(this DependencyObject d, Type type)
    {
      if (d == null)
      {
        throw new ArgumentNullException("d");
      }

      DependencyObject dpo = VisualTreeHelper.GetParent(d);

      while (dpo != null && !type.IsInstanceOfType(dpo))
      {
        DependencyObject parent = VisualTreeHelper.GetParent(dpo);
        if (parent != null)
        {
          dpo = parent;
        }
        else
        {
          if (!(dpo is FrameworkElement))
          {
            break;
          }
          dpo = (dpo as FrameworkElement).Parent;
        }
      }

      return dpo;
    }

    public static Binding GetBinding(this DependencyObject obj, DependencyProperty dp)
    {
      return BindingOperations.GetBinding(obj, dp);
    }

    public static void UpdateBindingSource(this DependencyObject obj, DependencyProperty dp)
    {
      var bindingExp = BindingOperations.GetBindingExpression(obj, dp);
      if (bindingExp != null)
      {
        bindingExp.UpdateSource();
      }
    }
  }
}