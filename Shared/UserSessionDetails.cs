namespace TrackerSafe.Shared
{
  public class UserSessionDetails
  {
    public UserSessionDetails()
    {
    }
    public UserSessionDetails(string userId, string userName)
    {
      UserId = userId;
      UserName = userName;
    }
    public string UserId { get; set; }
    public string UserName { get; set; }
  }
}