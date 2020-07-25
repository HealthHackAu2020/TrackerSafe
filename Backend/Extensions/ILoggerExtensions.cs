using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TrackerSafe.Backend.Shared;

namespace TrackerSafe.Backend.Extensions
{
  public static class ILoggerExtensions
  {
    public static void LogFunctionStart(this ILogger log, HttpRequest req)
    {
      var sessionId = "";
      if (req != null && req.Headers.ContainsKey(Constants.HttpHeaderSessionId))
      {
        sessionId = req.Headers[Constants.HttpHeaderSessionId].ToString();
      }
      log.LogInformation("Starting Function; SessionId: {SessionId}", sessionId);
    }
  }
}