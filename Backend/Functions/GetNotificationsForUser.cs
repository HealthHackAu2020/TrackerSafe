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
  public class GetNotificationsForUser
  {
    private IUserDataStore _userDataStore;
    private IAccessTokenProvider _accessTokenProvider;
    public GetNotificationsForUser(IUserDataStore userDataStore, IAccessTokenProvider accessTokenProvider)
    {
      _userDataStore = userDataStore;
      _accessTokenProvider = accessTokenProvider;
    }

    [FunctionName("GetNotificationsForUser")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
      log.LogFunctionStart(req);
      try
      {
        var res = SecurityFunctionHelper.GetLoggedInUser(_accessTokenProvider, req, log);
        log.LogInformation("Running as {UserName}", res.UserName);

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<GetNotificationsForUserRequest>(requestBody);
        if (data == null)
        {
          throw new ApplicationException($"Unable to get JSON from {requestBody}");
        }

        var user = await _userDataStore.GetByUserNameAsync(res.UserName);
        if (user == null)
        {
          throw new ApplicationException($"Failed - user does not exist in database '{res.UserName}'");
        }

        if (user.Notifications == null)
        {
          user.Notifications = new List<Notification>();
        }

        var result = new GetNotificationsForUserResponse(user.Notifications);

        return new OkObjectResult(result);
      }
      catch (UnauthorizedUserException)
      {
        return new UnauthorizedResult();
      }

    }
  }
}
