using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using TrackerSafe.Shared;
namespace TrackerSafe.Backend.Security
{
  public static class SecurityFunctionHelper
  {
    public static UserSessionDetails GetLoggedInUser(IAccessTokenProvider accessTokenProvider, HttpRequest req, ILogger log)
    {
      var tokenRes = accessTokenProvider.ValidateToken(req);
      switch (tokenRes.Status)
      {
        case AccessTokenStatus.Valid:
          var userName = tokenRes.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          return new UserSessionDetails(userName);
        case AccessTokenStatus.NoToken:
          log.LogInformation("AccessTokenStatus.NoToken: returning UnauthorizedResult");
          throw new UnauthorizedUserException();
        case AccessTokenStatus.Expired:
          log.LogInformation("AccessTokenStatus.Expired: returning UnauthorizedResult");
          throw new UnauthorizedUserException();
        case AccessTokenStatus.Error:
          log.LogInformation("AccessTokenStatus.Error: throwing {ErrorMessage}", tokenRes.Exception.Message);
          throw tokenRes.Exception;
        default:
          throw new ApplicationException($"Unknown AccessTokenStatus: {tokenRes.Status}");
      }
    }
  }
}
