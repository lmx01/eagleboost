// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 01 10:42 PM

namespace eagleboost.shell.Tools
{
  using System;

  /// <summary>
  /// IApplicationActivationTracker
  /// </summary>
  public interface IApplicationActivationTracker
  {
    #region Methods
    void Start();

    void Stop();
    #endregion Methods

    #region Events
    event EventHandler Activated;

    event EventHandler Deactivated;
    #endregion Events
  }

  /// <summary>
  /// ApplicationActivationTracker - Setup WinEventHook to look at the EVENT_SYSTEM_FOREGROUND event
  /// </summary>
  public partial class ApplicationActivationTracker : IApplicationActivationTracker
  {
    #region IApplicationActivationTracker
    public void Start()
    {
      ActivationTracker.Activated += HandleActivated;
      ActivationTracker.Deactivated += HandleDeactivated;
      ActivationTracker.Start();
    }

    public void Stop()
    {
      ActivationTracker.Activated -= HandleActivated;
      ActivationTracker.Deactivated -= HandleDeactivated;
      ActivationTracker.Stop();
    }

    public event EventHandler Activated;

    private void RaiseActivated(object sender)
    {
      var handler = Activated;
      if (handler != null)
      {
        handler(sender, EventArgs.Empty);
      }
    }

    public event EventHandler Deactivated;

    private void RaiseDeactivated()
    {
      var handler = Deactivated;
      if (handler != null)
      {
        handler(this, EventArgs.Empty);
      }
    }
    #endregion IApplicationActivationTracker

    #region Event Handlers
    private void HandleActivated(object sender, EventArgs e)
    {
      RaiseActivated(sender);
    }
    private void HandleDeactivated(object sender, EventArgs e)
    {
      RaiseDeactivated();
    }
    #endregion Event Handlers
  }
}
