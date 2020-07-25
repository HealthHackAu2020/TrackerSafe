namespace TrackerSafe.Backend.Shared
{
  public class UserSessionDetails
  {
    public UserSessionDetails()
    {
    }
    public UserSessionDetails(string userName)
    {
      UserName = userName;
    }
    public string UserName { get; set; }
  }
}