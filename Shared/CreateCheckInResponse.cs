namespace TrackerSafe.Shared
{
  public class CreateCheckInResponse
  {

    public CreateCheckInResponse()
    {
    }
    public CreateCheckInResponse(bool successful, string message)
    {
      Successful = successful;
      Message = message;
    }
    public bool Successful { get; set; }
    public string Message { get; set; }

  }
}