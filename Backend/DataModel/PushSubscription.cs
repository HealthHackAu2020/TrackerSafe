using System;

namespace TrackerSafe.Backend.DataModel
{
  public class PushSubscription
  {
    public PushSubscription()
    {   
    }
    public PushSubscription(string deviceId, string url, string p256dh, string auth)
    {
        Id = Guid.NewGuid();
        DeviceId = deviceId;
        Url = url;
        P256dh = p256dh;
        Auth = auth;
    }

    public Guid Id { get; set; }
    public string DeviceId { get; set; }
    public string Url { get; set; }
    public string P256dh { get; set; }
    public string Auth { get; set; }
  }
}