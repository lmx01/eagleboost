// Author : Shuo Zhang
// 
// Creation :2018-03-22 21:43

namespace eagleboost.UserExperience.Controls
{
  using System.AddIn.Pipeline;
  using System.Windows;
  using System.Windows.Controls;
  using eagleboost.presentation.Extensions;
  using eagleboost.UserExperience.Threading;

  /// <summary>
  /// StaThreadContainer
  /// </summary>
  public abstract class StaThreadContainer<TParams, TState> : ContentControl
  {
    #region Dependency Properties
    #region InitializationParams
    public static readonly DependencyProperty InitializationParamsProperty = DependencyProperty.Register(
      "InitializationParams", typeof(TParams), typeof(StaThreadContainer<TParams, TState>), new PropertyMetadata(OnInitializationParamsChanged));

    public TParams InitializationParams
    {
      get { return (TParams)GetValue(InitializationParamsProperty); }
      set { SetValue(InitializationParamsProperty, value); }
    }

    private static void OnInitializationParamsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      ((StaThreadContainer<TParams, TState>)obj).OnInitializationParamsChanged((TParams)e.NewValue);
    }
    #endregion InitializationParams

    #region State
    public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
      "State", typeof(TState), typeof(StaThreadContainer<TParams, TState>));

    public TState State
    {
      get { return (TState)GetValue(StateProperty); }
      set { SetValue(StateProperty, value); }
    }
    #endregion State
    #endregion Dependency Properties

    #region Public Properties
    public string ThreadName { get; set; }
    #endregion Public Properties

    #region Event Handlers
    private void OnInitializationParamsChanged(TParams initParams)
    {
      DispatcherViewFactory.CreateViewContract(ThreadName, () => CreateControl(initParams)).ContinueWith(
        t => Content = FrameworkElementAdapters.ContractToViewAdapter(t.Result), UiThread.Current.TaskScheduler);
    }
    #endregion Event Handlers

    #region Protected Methods
    protected void UpdateState(TState state)
    {
      Dispatcher.CheckedInvoke(() => SetCurrentValue(StateProperty, state));
    }
    #endregion Protected Methods

    #region Virtuals
    protected abstract FrameworkElement CreateControl(TParams initParams);
    #endregion Virtuals
  }

  /// <summary>
  /// StaThreadContainer
  /// </summary>
  public abstract class StaThreadContainer : StaThreadContainer<object, object>
  {
  }
}