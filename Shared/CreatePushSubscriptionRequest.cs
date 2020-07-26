using System;
namespace TrackerSafe.Shared
{
  public class CreatePushSubscriptionRequest
  {
    public CreatePushSubscriptionRequest()
    {
    }
    public CreatePushSubscriptionRequest(string deviceId, string url, string p256dh, string auth)
    {
        DeviceId = deviceId;
        Url = url;
        P256dh = p256dh;
        Auth = auth;
    }
    public string DeviceId { get; set; }
    public string Url { get; set; }
    public string P256dh { get; set; }
    public string Auth { get; set; }
  }
}