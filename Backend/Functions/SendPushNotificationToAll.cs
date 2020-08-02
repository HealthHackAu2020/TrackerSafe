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
  public class SendPushNotificationToAll
  {
    private IUserDataStore _userDataStore;
    private IAccessTokenProvider _accessTokenProvider;
    private readonly Random _random = new Random();
    public SendPushNotificationToAll(IUserDataStore userDataStore, IAccessTokenProvider accessTokenProvider)
    {
      _userDataStore = userDataStore;
      _accessTokenProvider = accessTokenProvider;
    }

    [FunctionName("SendPushNotificationToAll")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Admin, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
      log.LogInformation("Running Admin Function");

      var message = await new StreamReader(req.Body).ReadToEndAsync();

      var numUsersSentTo = 0;

      var allUsers = await _userDataStore.GetAllAsync();
      foreach (var user in allUsers)
      {
        var newNotification = new Notification();
        newNotification.Id = Guid.NewGuid();
        newNotification.CreatedDateUtc = DateTime.UtcNow;
        newNotification.TypeOfNotification = Notification.NotificationType.Info;
        newNotification.Message = message;
        newNotification.FullText = message;
        user.Notifications.Add(newNotification);
        await _userDataStore.UpdateAsync(user.Id, user);

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
              log.LogWarning("Unable to send notifcation to user {UserName} on device {DeviceId}: {ExceptionMessage}", user.UserNameDisplay, currSub.DeviceId, ex.Message);
              if (ex.Message == "Subscription no longer valid")
              {
                idsToRemove.Add(currSub.Id);
              }
            }
          }

          if (wasSent)
          {
            numUsersSentTo++;
            var updUser = await _userDataStore.GetByUserNameAsync(user.UserNameLower);
            var notificationToUpdate = updUser.Notifications.Single(n => n.Id == newNotification.Id);
            notificationToUpdate.PushSentAtDateUtc = DateTime.UtcNow;
            await _userDataStore.UpdateAsync(user.Id, updUser);
          }

          if (idsToRemove.Count > 0)
          {
            //If we have other subscriptions for this device, remove them, newst one only kept
            var updUser = await _userDataStore.GetByUserNameAsync(user.UserNameLower);
            updUser.PushSubscriptions.RemoveAll(p => idsToRemove.Contains(p.Id));
            await _userDataStore.UpdateAsync(user.Id, updUser);
          }
        }
      }

      return new OkObjectResult(numUsersSentTo);
    }
  }
}
