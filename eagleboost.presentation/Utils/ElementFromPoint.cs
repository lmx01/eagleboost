namespace eagleboost.presentation.Utils
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows;
  using System.Windows.Input;
  using System.Windows.Media;

  public static class ElementFromPointExt
  {
    public static T GetElementFromPoint<T>(this UIElement element, MouseButtonEventArgs args)
    {
      var pt = args.GetPosition(element);
      return element.GetElementFromPoint<T>(pt);
    }

    public static T GetElementFromPoint<T>(this UIElement element, Point pt)
    {
      var util = new ElementFromPoint<T>(element, pt);
      return util.Element;
    }

    public class ElementFromPoint<T>
    {
      #region Declarations
      private readonly List<DependencyObject> _hitResultsList = new List<DependencyObject>();
      #endregion Declarations


      internal ElementFromPoint(UIElement element, Point pt)
      {
        VisualTreeHelper.HitTest(element, null, HitTestCallback, new PointHitTestParameters(pt));

        Element = _hitResultsList.OfType<T>().FirstOrDefault();
      }

      internal T Element { get; set; }

      private HitTestResultBehavior HitTestCallback(HitTestResult result)
      {
        // Add the hit test result to the list that will be processed after the enumeration.
        _hitResultsList.Add(result.VisualHit);

        // Set the behavior to return visuals at all z-order levels.
        return HitTestResultBehavior.Continue;
      }
    }
  }
}