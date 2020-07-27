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
  public class SignUpUser
  {
    private IUserDataStore _userDataStore;
    private IAccessTokenProvider _accessTokenProvider;
    private readonly Random _random = new Random();  
    public SignUpUser(IUserDataStore userDataStore, IAccessTokenProvider accessTokenProvider)
    {
      _userDataStore = userDataStore;
      _accessTokenProvider = accessTokenProvider;
    }


    [FunctionName("SignUpUser")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
      log.LogFunctionStart(req);

      var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
      var data = JsonConvert.DeserializeObject<SignUpUserRequest>(requestBody);
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
      else if (data.Password != data.ConfirmPassword)
      {
        message = "Your passwords do not match";
      }
      else if ((await _userDataStore.GetByUserNameAsync(data.UserName)) != null)
      {
        message = $"Username {data.UserName} is not available, please use a different one";
      }
      else
      {
        var user = new User();
        user.UserNameLower = data.UserName?.Trim()?.ToLower();
        user.UserNameDisplay = data.UserName?.Trim();
        user.SuppliedReferralCode = data.ReferralCode.Trim();
        var pwHasher = new PasswordHasher<User>();
        user.PwHash = pwHasher.HashPassword(user, data.Password);
        var createdUser = await _userDataStore.CreateAsync(user);
        log.LogInformation("Created user with id '{UserId}'", createdUser.Id);

        //get a unique referral code.  Just for fun let's make it a 6 digit number so it's easy to remember
        //but can't overlap...
        var foundUnique = false;
        while (!foundUnique)
        {
          var referralCode = GenerateRandomNumber(100000, 999999).ToString();
          var existing = await _userDataStore.GetByReferralCodeAsync(referralCode);
          if (existing == null)
          {
            log.LogInformation("Found unique referral code '{ReferralCode}' for user id '{UserId}' with name '{UserName}'", referralCode, createdUser.Id, createdUser.UserNameDisplay);
            foundUnique = true;
            createdUser.MyReferralCode = referralCode;
            await _userDataStore.UpdateAsync(createdUser.Id, createdUser);
          }
        }

        jwt = _accessTokenProvider.GenerateToken(createdUser.Id, createdUser.UserNameDisplay);

        successful = true;
      }

      log.LogInformation("Result for '{UserName}' successful: {Successful}, message: {Message}", data.UserName, successful, message);
      return new OkObjectResult(new SignUpUserResponse(data.UserName, successful, message, jwt));
    }

    public int GenerateRandomNumber(int min, int max)  
    {  
      return _random.Next(min, max);  
    }  
  }
}
