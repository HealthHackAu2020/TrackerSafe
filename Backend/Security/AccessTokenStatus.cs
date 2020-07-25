// https://github.com/BenMorris/FunctionsCustomSercuity/blob/master/dependency-injection/AccessTokens/AccessTokenStatus.cs
namespace TrackerSafe.Backend.Security
{
  public enum AccessTokenStatus
  {
    Valid,
    Expired,
    Error,
    NoToken
  }
}