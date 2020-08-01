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
  public class MarkAllNotificationsAsRead
  {
    private IUserDataStore _userDataStore;
    private IAccessTokenProvider _accessTokenProvider;
    private readonly Random _random = new Random();  
    public MarkAllNotificationsAsRead(IUserDataStore userDataStore, IAccessTokenProvider accessTokenProvider)
    {
      _userDataStore = userDataStore;
      _accessTokenProvider = accessTokenProvider;
    }

    [FunctionName("MarkAllNotificationsAsRead")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
      log.LogFunctionStart(req);
      try
      {
        var res = SecurityFunctionHelper.GetLoggedInUser(_accessTokenProvider, req, log);
        log.LogInformation("Running as {UserName}", res.UserName);

        var user = await _userDataStore.GetByUserNameAsync(res.UserName);
        if (user == null)
        {
          throw new ApplicationException($"Failed - user does not exist in database '{res.UserName}'");
        }

        if (user.Notifications == null)
        {
          user.Notifications = new List<Notification>();
        }
        foreach(var currNotification in user.Notifications)
        {
          if (currNotification.IsRead == false)
          {
            currNotification.IsRead = true;
          }
        }
        
        await _userDataStore.UpdateAsync(res.UserId, user);

        return new OkObjectResult(true);
      }
      catch (UnauthorizedUserException)
      {
        return new UnauthorizedResult();
      }

    }
  }
}
