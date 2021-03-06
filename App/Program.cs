using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BlazorStrap;
using Blazored.LocalStorage;
using Blazor.Extensions.Logging;
using AspNetMonsters.Blazor.Geolocation;
using Blazored.Modal;

namespace app
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      var builder = WebAssemblyHostBuilder.CreateDefault(args);
      builder.RootComponents.Add<App>("app");

      builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

      builder.Services.AddBlazoredLocalStorage();

      builder.Services.AddLogging(builder => builder
          .AddBrowserConsole()
          .SetMinimumLevel(LogLevel.Information)
      );

      builder.Services.AddSingleton<LocationService>();

      builder.Services.AddBlazoredModal();

      await builder.Build().RunAsync();
    }
  }
}
