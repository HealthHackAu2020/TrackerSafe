using System.Data;
using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TrackerSafe.Backend.Security;
using TrackerSafe.Backend.DataModel;

[assembly: FunctionsStartup(typeof(TrackerSafe.Backend.Startup))]

namespace TrackerSafe.Backend
{
  public class Startup : FunctionsStartup
  {
    public override void Configure(IFunctionsHostBuilder builder)
    {
      //for ExcelDataReader on DotNetCore
      System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

      // Get the configuration files for the OAuth token issuer
      var issuerToken = Environment.GetEnvironmentVariable("IssuerToken");
      var audience = Environment.GetEnvironmentVariable("Audience");
      var issuer = Environment.GetEnvironmentVariable("Issuer");

      // Register the access token provider as a singleton
      builder.Services.AddSingleton<IAccessTokenProvider, AccessTokenProvider>(s => new AccessTokenProvider(issuerToken, audience, issuer));

      var mongoConnString = Environment.GetEnvironmentVariable("MongoConnectionString");
      var mongoDbName = Environment.GetEnvironmentVariable("MongoDatabaseName");
      builder.Services.AddSingleton<IUserDataStore, UserDataStore>(s => new UserDataStore(mongoConnString, mongoDbName));
      builder.Services.AddSingleton<ICheckInDataStore, CheckInDataStore>(s => new CheckInDataStore(mongoConnString, mongoDbName));

      var config = new ConfigurationBuilder()
        //.SetBasePath(context.FunctionAppDirectory)
        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();

      builder.Services.AddSingleton<IConfiguration>((s) =>
      {
        return config;
      });
    }
  }
}