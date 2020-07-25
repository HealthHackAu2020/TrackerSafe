namespace TrackerSafe.Shared
{
  public class LogInUserResponse
  {

    public LogInUserResponse()
    {
    }
    public LogInUserResponse(string userName, bool successful, string message)
    {
      UserName = userName;
      Successful = successful;
      Message = message;
    }
    public string UserName { get; set; }
    public bool Successful { get; set; }
    public string Message { get; set; }

  }
}