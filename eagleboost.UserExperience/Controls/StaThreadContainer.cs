// Author : Shuo Zhang
// 
// Creation :2018-03-22 21:43

namespace eagleboost.UserExperience.Controls
{
  using System;
  using System.AddIn.Pipeline;
  using System.Windows;
  using System.Windows.Controls;
  using eagleboost.UserExperience.Threading;

  /// <summary>
  /// StaThreadContainer
  /// </summary>
  public class StaThreadContainer : ContentControl
  {
    #region ctors
    public StaThreadContainer()
    {
      DataContextChanged += HandleDataContextChanged;
    }
    #endregion ctors

    #region Dependency Properties
    #region ContentStyle
    public static readonly DependencyProperty ContentStyleProperty = DependencyProperty.Register(
      "ContentStyle", typeof(Style), typeof(StaThreadContainer), new PropertyMetadata(OnContentStyleChanged));

    public Style ContentStyle
    {
      get { return (Style) GetValue(ContentStyleProperty); }
      set { SetValue(ContentStyleProperty, value); }
    }

    private static void OnContentStyleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((StaThreadContainer)obj).OnContentStyleChanged();
    }
    #endregion ContentStyle
    #endregion Dependency Properties

    #region Public Properties
    public Type DataContextType { get; set; }

    public string ThreadName { get; set; }
    #endregion Public Properties

    #region Event Handlers
    private void HandleDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      var context = e.NewValue;
      //if (DataContextType.IsInstanceOfType(context))
      {
        DispatcherViewFactory.CreateViewContract(ThreadName, () => new TextBlock{Text = string.Format("I'm running on thread '{0}", ThreadName)}).ContinueWith(t =>
        {
          var element = FrameworkElementAdapters.ContractToViewAdapter(t.Result);
          element.DataContext = context;
          Content = element; 
        }, UiThread.Current.TaskScheduler);
      }
    }
    #endregion Event Handlers

    #region Private Methods
    private void OnContentStyleChanged()
    {
    }
    #endregion Private Methods
  }
}