// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-22 11:07 PM

namespace eagleboost.presentation.Controls
{
  using System;
  using System.Windows;
  using System.Windows.Documents;
  using System.Windows.Media;
  using eagleboost.presentation.Extensions;


  public class OverlayAdorner : Adorner, IDisposable
  {
    #region Declarations
    private FrameworkElement _overlayElement;
    private AdornerLayer _layer; /// 
    #endregion Declarations

    #region ctors
    public static IDisposable Overlay(FrameworkElement elementToAdorn, FrameworkElement adorningElement)
    {
      return new OverlayAdorner(elementToAdorn, adorningElement);
    }

    private OverlayAdorner(FrameworkElement target, FrameworkElement overlayElement): base(target)
    {
      target.SetupLoaded(() =>
      {
        _layer = AdornerLayer.GetAdornerLayer(target);
        _layer.Add(this);
      });

      _overlayElement = overlayElement;
      if (overlayElement != null)
      {
        AddVisualChild(overlayElement);
      }
      Focusable = true;
    }
    #endregion ctors
    protected override int VisualChildrenCount
    {
      get { return _overlayElement == null ? 0 : 1; }
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      if (_overlayElement != null)
      {
        var adorningPoint = new Point(0, 0);
        _overlayElement.Arrange(new Rect(adorningPoint, finalSize));
      }
      return finalSize;
    }

    protected override Visual GetVisualChild(int index)
    {
      if (index == 0 && _overlayElement != null)
      {
        return _overlayElement;
      }

      return base.GetVisualChild(index);
    }

    public void Dispose()
    {
      RemoveVisualChild(_overlayElement);
      if (_layer != null)
      {
        _layer.Remove(this);
      }
    }
  }
}