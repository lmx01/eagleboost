// Author : Shuo Zhang
// 
// Creation :2018-03-22 21:43

namespace eagleboost.UserExperience.Controls
{
  using System;
  using System.AddIn.Pipeline;
  using System.Windows;
  using System.Windows.Controls;
  using eagleboost.presentation.Extensions;
  using eagleboost.UserExperience.Threading;

  /// <summary>
  /// StaThreadContainer
  /// </summary>
  public abstract class StaThreadContainer : ContentControl
  {
    #region Dependency Properties
    #region InitializationParams
    public static readonly DependencyProperty InitializationParamsProperty = DependencyProperty.Register(
      "InitializationParams", typeof(object), typeof(StaThreadContainer), new PropertyMetadata(OnInitializationParamsChanged));

    public object InitializationParams
    {
      get { return GetValue(InitializationParamsProperty); }
      set { SetValue(InitializationParamsProperty, value); }
    }

    private static void OnInitializationParamsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((StaThreadContainer)obj).OnInitializationParamsChanged(e.NewValue);
    }
    #endregion InitializationParams

    #region State
    public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
      "State", typeof(object), typeof(StaThreadContainer));

    public object State
    {
      get { return GetValue(StateProperty); }
      set { SetValue(StateProperty, value); }
    }
    #endregion State
    #endregion Dependency Properties

    #region Public Properties
    public string ThreadName { get; set; }
    #endregion Public Properties

    #region Event Handlers
    private void OnInitializationParamsChanged(object initParams)
    {
      this.SetupDataContextChanged(e =>
      {
        DispatcherViewFactory.CreateViewContract(ThreadName, () => CreateControl(initParams)).ContinueWith(t =>
        {
          Content = FrameworkElementAdapters.ContractToViewAdapter(t.Result);
        }, UiThread.Current.TaskScheduler);
      });
    }
    #endregion Event Handlers

    #region Protected Methods
    protected void UpdateState(object state)
    {
      Dispatcher.CheckedInvoke(() => SetCurrentValue(StateProperty, state));
    }
    #endregion Protected Methods

    #region Virtuals
    protected abstract FrameworkElement CreateControl(object initParams);
    #endregion Virtuals
  }
}