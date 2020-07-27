namespace TrackerSafe.Shared
{
  public class SignUpUserResponse
  {

    public SignUpUserResponse()
    {
    }
    public SignUpUserResponse(string userName, bool successful, string message, string jwt)
    {
      UserName = userName;
      Successful = successful;
      Message = message;
      Jwt = jwt;
    }
    public string UserName { get; set; }
    public bool Successful { get; set; }
    public string Message { get; set; }
    public string Jwt { get; set; }

  }
}