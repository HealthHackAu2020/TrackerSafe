using System.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using TrackerSafe.Backend.Extensions;
using TrackerSafe.Backend.DataModel;
using TrackerSafe.Backend.Security;
using TrackerSafe.Shared;

namespace TrackerSafe.Backend.Functions
{
  public class GetLoggedInUser
  {
    private IAccessTokenProvider _accessTokenProvider;
    private readonly Random _random = new Random();  
    public GetLoggedInUser(IAccessTokenProvider accessTokenProvider)
    {
      _accessTokenProvider = accessTokenProvider;
    }

    [FunctionName("GetLoggedInUser")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
      log.LogFunctionStart(req);
      try
      {
        var res = SecurityFunctionHelper.GetLoggedInUser(_accessTokenProvider, req, log);
        log.LogInformation("Running as {UserName}", res.UserName);
        var result = new GetLoggedInUserResponse(res.UserName);
        return new OkObjectResult(result);
      }
      catch (UnauthorizedUserException)
      {
        return new UnauthorizedResult();
      }
    }
  }
}
