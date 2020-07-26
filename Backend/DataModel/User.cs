using System.Diagnostics;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrackerSafe.Backend.DataModel
{
  public class User
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string UserNameLower { get; set; }
    public string UserNameDisplay { get; set; }
    public string SuppliedReferralCode { get; set; }
    public string MyReferralCode { get; set; }
    public string PwHash { get; set; }
  }
}