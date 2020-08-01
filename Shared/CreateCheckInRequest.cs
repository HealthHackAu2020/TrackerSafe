using System;
namespace TrackerSafe.Shared
{
  public class CreateCheckInRequest
  {
    public CreateCheckInRequest()
    {
    }
    public CreateCheckInRequest(DateTime checkInDateUtc, double? latitude, double? longitude, string placeName)
    {
        CheckInDateUtc = checkInDateUtc;
        Latitude = latitude;
        Longitude = longitude;
        PlaceName = placeName;
    }
    public DateTime? CheckInDateUtc { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string PlaceName { get; set; }
  }
}