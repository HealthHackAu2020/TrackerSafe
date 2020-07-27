namespace TrackerSafe.Shared
{
  public class AppState
  {
    public AppState()
    {
    }

    public FeatureState PushNotificationState { get; set; }
    public FeatureState LocationState { get; set; }
    public FeatureState InstalledState { get; set; }

    public enum FeatureState
    {
      None = 0,
      OptedOut = 1,
      Configured = 2
    }
  }
}