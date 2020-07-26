namespace TrackerSafe.Shared
{
  public class LogInUserRequest
  {
    public LogInUserRequest()
    {
    }
    public LogInUserRequest(string userName, string password)
    {
      UserName = userName;
      Password = password;
    }
    public string UserName { get; set; }
    public string Password { get; set; }
  }
}