namespace TrackerSafe.Shared
{
  public class GetMapsTokenResponse
  {
    public GetMapsTokenResponse()
    {
    }
    public GetMapsTokenResponse(string mapsToken)
    {
      MapsToken = mapsToken;
    }
    public string MapsToken { get; set; }
  }
}