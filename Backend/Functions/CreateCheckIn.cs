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
  public class CreateCheckIn
  {
    private ICheckInDataStore _checkInDataStore;
    private IAccessTokenProvider _accessTokenProvider;
    private readonly Random _random = new Random();  
    public CreateCheckIn(ICheckInDataStore checkInDataStore, IAccessTokenProvider accessTokenProvider)
    {
      _checkInDataStore = checkInDataStore;
      _accessTokenProvider = accessTokenProvider;
    }

    [FunctionName("CreateCheckIn")]
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
        var data = JsonConvert.DeserializeObject<CreateCheckInRequest>(requestBody);
        if (data == null)
        {
          throw new ApplicationException($"Unable to get JSON from {requestBody}");
        }
        
        var isSuccessful = false;
        var message  = "";
        if (data.CheckInDateUtc == null)
        {
          message = "You must supply a check in date";
        }
        else if (data.Latitude == null || data.Latitude.Value < -90 || data.Latitude.Value > 90)
        {
            message = "Latitude must be between -90 and 90 degrees inclusive";
        }
        else if (data.Longitude == null || data.Longitude.Value < -180 || data.Longitude.Value > 180)
        {
            message = "Longitude must be between -180 and 180 degrees inclusive";
        }
        else
        {
          isSuccessful = true;
          var record = new CheckIn
          {
            UserId = res.UserId,
            CreatedDateUtc = DateTime.UtcNow,
            CheckInDateUtc = data.CheckInDateUtc.Value,
            Latitude = data.Latitude.Value,
            Longitude = data.Longitude.Value,
            PlaceName = data.PlaceName?.Trim()
          };
          await _checkInDataStore.CreateAsync(record);
        }

        return new OkObjectResult(new CreateCheckInResponse(isSuccessful, message));
      }
      catch (UnauthorizedUserException)
      {
        return new UnauthorizedResult();
      }
    }
  }
}
