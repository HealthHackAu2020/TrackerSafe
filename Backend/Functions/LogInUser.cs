using System;
using System.IO;
using System.Threading.Tasks;
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
  public class LogInUser
  {
    private IUserDataStore _userDataStore;
    private IAccessTokenProvider _accessTokenProvider;
    private readonly Random _random = new Random();  
    public LogInUser(IUserDataStore userDataStore, IAccessTokenProvider accessTokenProvider)
    {
      _userDataStore = userDataStore;
      _accessTokenProvider = accessTokenProvider;
    }

    [FunctionName("LogInUser")]
    public async Task<IActionResult> Run(
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
      var jwt = "";

      if (string.IsNullOrWhiteSpace(data.UserName))
      {
        message = "You must supply a Username";
      }
      else if (string.IsNullOrWhiteSpace(data.Password))
      {
        message = "You must supply a Password";
      }
      else
      {
        var user = await _userDataStore.GetByUserNameAsync(data.UserName);
        if (user == null)
        {
          log.LogInformation("Failed - user does not exist '{UserName}'", data.UserName);
          message = "Invalid Username and/or Password";
        }
        else
        {
          var pwHasher = new PasswordHasher<User>();
          var pwResult = pwHasher.VerifyHashedPassword(user, user.PwHash, data.Password);
          if (pwResult == PasswordVerificationResult.Failed)
          {
            log.LogInformation("Failed - password mismatch '{UserName}'", data.UserName);
            message = "Invalid Username and/or Password";
          }
          else if (pwResult == PasswordVerificationResult.SuccessRehashNeeded 
            || pwResult == PasswordVerificationResult.Success)
          { 
            jwt = _accessTokenProvider.GenerateToken(user.Id, user.UserNameDisplay);
            successful = true;
          }
        }
      }

      log.LogInformation("Result for '{UserName}' successful: {Successful}, message: {Message}", data.UserName, successful, message);
      return new OkObjectResult(new LogInUserResponse(data.UserName, successful, message, jwt));
    }

    public int GenerateRandomNumber(int min, int max)  
    {  
      return _random.Next(min, max);  
    }  
  }
}
