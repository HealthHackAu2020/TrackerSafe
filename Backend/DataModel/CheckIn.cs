using System;
using System.Diagnostics;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace TrackerSafe.Backend.DataModel
{
  public class CheckIn
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime CheckInDateUtc { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string PlaceName { get; set; }
  }
}