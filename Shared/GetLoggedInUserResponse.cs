namespace TrackerSafe.Shared
{
  public class GetLoggedInUserResponse
  {
    public GetLoggedInUserResponse()
    {
    }
    public GetLoggedInUserResponse(string userName)
    {
      UserName = userName;
    }
    public string UserName { get; set; }
  }
}