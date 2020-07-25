namespace TrackerSafe.Shared
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