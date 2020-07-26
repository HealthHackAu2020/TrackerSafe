namespace TrackerSafe.Shared
{
  public class SignUpUserResponse
  {

    public SignUpUserResponse()
    {
    }
    public SignUpUserResponse(string userName, bool successful, string message)
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