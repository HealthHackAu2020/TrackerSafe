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

namespace TrackerSafe.Backend.Functions
{
  public static class GetLoggedInUser
  {
    [FunctionName("GetLoggedInUser")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
        ILogger log)
    {
      log.LogFunctionStart(req);

      return new OkObjectResult("I don't work yet :P");
    }
  }
}
