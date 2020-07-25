using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TrackerSafe.Backend.Extensions;
using TrackerSafe.Shared;

namespace TrackerSafe.Backend.Functions
{
  public static class LogInUser
  {
    [FunctionName("LogInUser")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
      log.LogFunctionStart(req);

      var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
      var data = JsonConvert.DeserializeObject<LogInUserRequest>(requestBody);
      if (data == null)
      {
        throw new ApplicationException($"Unable to get JSON from {requestBody}");
      }
      var successful = false;
      var message  = "";

      if (string.IsNullOrWhiteSpace(data.UserName))
      {
        message = "You must supply a username";
      }
      else if (string.IsNullOrWhiteSpace(data.Password))
      {
        message = "You must supply a password";
      }
      else if (data.Password != data.ConfirmPassword)
      {
        message = "Your passwords do not match";
      }
      else
      {
        successful = true;
      }

      return new OkObjectResult(new LogInUserResponse(data.UserName, successful, message));
    }
  }
}
