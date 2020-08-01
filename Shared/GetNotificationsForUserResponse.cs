using System.Collections.Generic;
namespace TrackerSafe.Shared
{
  public class GetNotificationsForUserResponse
  {

    public GetNotificationsForUserResponse()
    {
    }    
    public GetNotificationsForUserResponse(IList<Notification> notifications)
    {
        Notifications = notifications;
    }
    public IList<Notification> Notifications { get; set; }
  }
}