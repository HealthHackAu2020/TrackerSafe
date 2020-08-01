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
  public class SendPushNotificationToSelf
  {
    private IUserDataStore _userDataStore;
    private IAccessTokenProvider _accessTokenProvider;
    private readonly Random _random = new Random();  
    public SendPushNotificationToSelf(IUserDataStore userDataStore, IAccessTokenProvider accessTokenProvider)
    {
      _userDataStore = userDataStore;
      _accessTokenProvider = accessTokenProvider;
    }

    [FunctionName("SendPushNotificationToSelf")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
      log.LogFunctionStart(req);
      try
      {
        var res = SecurityFunctionHelper.GetLoggedInUser(_accessTokenProvider, req, log);
        log.LogInformation("Running as {UserName}", res.UserName);

        var message = await new StreamReader(req.Body).ReadToEndAsync();

        var user = await _userDataStore.GetByUserNameAsync(res.UserName);
        if (user == null)
        {
          throw new ApplicationException($"Failed - user does not exist in database '{res.UserName}'");
        }

        if (user.Notifications == null)
        {
          user.Notifications = new List<Notification>();
        }
        var newNotification = new Notification();
        newNotification.Id = Guid.NewGuid();
        newNotification.CreatedDateUtc = DateTime.UtcNow;
        newNotification.TypeOfNotification = Notification.NotificationType.Info;
        newNotification.Message = message;
        newNotification.FullText = message;
        user.Notifications.Add(newNotification);
        await _userDataStore.UpdateAsync(res.UserId, user);

        var numSubscriptions = user.PushSubscriptions?.Count;

        var idsToRemove = new List<Guid>();
        if (user.PushSubscriptions != null && user.PushSubscriptions.Count > 0)
        {
          var wasSent = false;
          var publicKey = Environment.GetEnvironmentVariable("PushNotificationPublicKey");
          var privateKey = Environment.GetEnvironmentVariable("PushNotificationPrivateKey");
          var vapidDetails = new WebPush.VapidDetails("mailto:<someone@example.com>", publicKey, privateKey);
          var webPushClient = new WebPush.WebPushClient();
          foreach (var currSub in user.PushSubscriptions)
          {
            var pushSubscription = new WebPush.PushSubscription(currSub.Url, currSub.P256dh, currSub.Auth);
            var payload = JsonConvert.SerializeObject(new
            {
              id = newNotification.Id.ToString(),
              message = newNotification.Message
            });
            try
            {
              await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);
              wasSent = true;
            }
            catch (Exception ex)
            {
              log.LogWarning("Unable to send notifcation to user {UserName} on device {DeviceId}: {ExceptionMessage}", res.UserName, currSub.DeviceId, ex.Message);
              if (ex.Message == "Subscription no longer valid")
              {
                idsToRemove.Add(currSub.Id);
              }
            }
          }

          if (wasSent)
          {
            user = await _userDataStore.GetByUserNameAsync(res.UserName);
            var notificationToUpdate = user.Notifications.Single(n => n.Id == newNotification.Id);
            notificationToUpdate.PushSentAtDateUtc = DateTime.UtcNow;
            await _userDataStore.UpdateAsync(res.UserId, user);
          }
        }

        if (idsToRemove.Count > 0)
        {
          //If we have other subscriptions for this device, remove them, newst one only kept
          user = await _userDataStore.GetByUserNameAsync(res.UserName);
          user.PushSubscriptions.RemoveAll(p => idsToRemove.Contains(p.Id));
          await _userDataStore.UpdateAsync(res.UserId, user);
        }

        return new OkObjectResult(numSubscriptions);
      }
      catch (UnauthorizedUserException)
      {
        return new UnauthorizedResult();
      }

    }
  }
}
