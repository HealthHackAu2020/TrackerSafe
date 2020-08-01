using System;

namespace TrackerSafe.Shared
{
  public class Notification
  {
    public Notification()
    {
    }

    public Guid Id { get; set; }
    public string Message { get; set; }
    public string FullText { get; set; }
    public bool IsRead { get; set; }

    public DateTime CreatedDateUtc { get; set; }

    public DateTime? PushSentAtDateUtc { get; set; }

    public NotificationType TypeOfNotification { get; set; }

    public enum NotificationType
    {
      Info = 0,
      Reminder = 1,
      GreenAlert = 2,
      YellowAlert = 3,
      RedAlert = 4
    }
  }
}