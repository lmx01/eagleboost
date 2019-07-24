// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-28 1:57 PM

namespace eagleboost.persistence.Services
{
  using System.IO;
  using System.IO.IsolatedStorage;
  using System.Windows;
  using eagleboost.core.Extensions;
  using eagleboost.core.Logging;
  using eagleboost.persistence.Contracts;
  using Newtonsoft.Json;

  /// <summary>
  /// ApplicationSettingsService
  /// </summary>
  public class ApplicationSettingsService : IApplicationSettingsService
  {
    #region Statics
    private static readonly ILoggerFacade Log = LoggerManager.GetLogger<ApplicationSettingsService>();
    #endregion Statics

    #region Declarations
    private static string _appName;
    private static Application _application;
    #endregion Declarations

    /// <summary>
    /// Nested
    /// </summary>
    private class Nested
    {
      static Nested()
      {
      }

      internal static readonly IApplicationSettingsService NestedInstance = new ApplicationSettingsService();
    }

    #region ctors
    public static IApplicationSettingsService Instance
    {
      get { return Nested.NestedInstance; }
    }

    private ApplicationSettingsService(string appName = null)
    {
      _appName = appName ?? "Default";
      _application = Application.Current;
      _application.Startup += HandleApplicationStartup;
      _application.Exit += HandleApplicationExit;
    }
    #endregion ctors

    #region IApplicationSettingsService
    public void Save<T>(string key, T setting) where T : class
    {
      var settingJson = JsonConvert.SerializeObject(setting);
      Log.Info("Save setting " + key + ": " + settingJson);
      _application.Properties[key] = settingJson;
    }

    public T Load<T>(string key) where T : class
    {
      T result;
      if (TryLoadState(key, out result))
      {
        return result;
      }

      return null;
    }
    #endregion IApplicationSettingsService

    #region Event Handlers
    private static void HandleApplicationStartup(object sender, StartupEventArgs e)
    {
      // Restore application-scope property from isolated storage
      var storage = IsolatedStorageFile.GetUserStoreForDomain();
      try
      {
        using (var stream = new IsolatedStorageFileStream(_appName, FileMode.Open, storage))
        {
          using (var reader = new StreamReader(stream))
          {
            // Restore each application-scope property individually
            while (!reader.EndOfStream)
            {
              var line = reader.ReadLine();
              if (line != null && line.HasValue())
              {
                var parts = line.Split('|');
                if (parts.Length == 2)
                {
                  _application.Properties[parts[0]] = parts[1];
                }
              }
            }
          }
        }
      }
      catch (FileNotFoundException ex)
      {
        // Handle when file is not found in isolated storage:
        // * When the first application session
        // * When file has been deleted
      }
    }

    private void HandleApplicationExit(object sender, ExitEventArgs e)
    {
      _application.Startup -= HandleApplicationStartup;
      _application.Exit -= HandleApplicationExit;

      // Persist application-scope property to isolated storage
      var storage = IsolatedStorageFile.GetUserStoreForDomain();
      using (var stream = new IsolatedStorageFileStream(_appName, FileMode.Create, storage))
      {
        using (var writer = new StreamWriter(stream))
        {
          // Persist each application-scope property individually
          foreach (string key in _application.Properties.Keys)
          {
            writer.WriteLine("{0}|{1}", key, _application.Properties[key]);
          }
        }
      }
    }
    #endregion Event Handlers

    #region Private Methods
    private static bool TryLoadState<T>(object key, out T state)
    {
      if (_application.Properties.Contains(key))
      {
        var str = (string)_application.Properties[key];
        state = JsonConvert.DeserializeObject<T>(str);
        Log.Info("Load setting " + key + ": " + str);
        return true;
      }

      state = default(T);
      Log.Info("Load default setting " + key + ": " + state);
      return false;
    }
    #endregion Private Methods
  }
}