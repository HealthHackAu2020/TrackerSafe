using System;
namespace TrackerSafe.Shared
{
  public class RemovePushSubscriptionRequest
  {
    public RemovePushSubscriptionRequest()
    {
    }
    public RemovePushSubscriptionRequest(string deviceId)
    {
        DeviceId = deviceId;
    }
    public string DeviceId { get; set; }
  }
}