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
using TrackerSafe.Shared;

namespace TrackerSafe.Backend.Functions
{
  public class SignUpUser
  {
    private IUserDataStore _userDataStore;
    public SignUpUser(IUserDataStore userDataStore)
    {
      _userDataStore = userDataStore;
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
        user.UserNameLower= data.UserName.ToLower();
        user.UserNameDisplay = data.UserName;
        user.SuppliedReferralCode = data.ReferralCode;
        var pwHasher = new PasswordHasher<User>();
        user.PwHash = pwHasher.HashPassword(user, data.Password);
        var createdUser = await _userDataStore.CreateAsync(user);
        log.LogInformation("Created user with id '{UserId}'", createdUser.Id);

        successful = true;
      }

      log.LogInformation("Result for '{UserName}' successful: {Successful}, message: {Message}", data.UserName, successful, message);
      return new OkObjectResult(new SignUpUserResponse(data.UserName, successful, message));
    }
  }
}
