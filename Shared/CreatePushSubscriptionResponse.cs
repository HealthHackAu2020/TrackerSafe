using System;
namespace TrackerSafe.Shared
{
  public class CreatePushSubscriptionResponse
  {

    public CreatePushSubscriptionResponse()
    {
    }    
    public CreatePushSubscriptionResponse(Guid pushSubscriptionId)
    {
        PushSubscriptionId = pushSubscriptionId;
    }
    public Guid PushSubscriptionId { get; set; }
  }
}