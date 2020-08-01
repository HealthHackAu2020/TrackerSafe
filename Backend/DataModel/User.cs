using System.Diagnostics;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

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
    public List<PushSubscription> PushSubscriptions { get; set; }
    public List<TrackerSafe.Shared.Notification> Notifications { get; set; }
  }
}