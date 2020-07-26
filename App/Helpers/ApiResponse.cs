using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using app.Extensions;

namespace app.Helpers
{
  public static class ApiResponse
  {
    public static void HandleUnsuccessfulApiResponse<T>(NavigationManager navManager, ILogger<T> logger, Exception ex)
    {
      logger.LogError(ex, "Error from API Call");
      if (ex is System.Net.Http.HttpRequestException)
      {
        var httpEx = ex as System.Net.Http.HttpRequestException;
        if (ex.Message.Contains("401") || ex.Message.Contains("403") || ex.Message.Contains("404"))
        {
          navManager.NavigateToPage(TrackerSafe.App.AppConstants.InvalidSession);
        }
        else if (ex.Message.Contains("500"))
        {
          navManager.NavigateToPage(TrackerSafe.App.AppConstants.UnexpectedError);
        }
      }
    }

    public static void HandleUnsuccessfulApiResponse<T>(NavigationManager navManager, ILogger<T> logger, HttpResponseMessage msg)
    {
      if ((int)msg.StatusCode >= 300 && (int)msg.StatusCode < 500)
      {
        logger.LogError($"Error from API Call - status {msg.StatusCode}");
        navManager.NavigateToPage(TrackerSafe.App.AppConstants.InvalidSession);
      }
      else if ((int)msg.StatusCode >= 500)
      {
        logger.LogError($"Error from API Call - status {msg.StatusCode}");
        navManager.NavigateToPage(TrackerSafe.App.AppConstants.UnexpectedError);
      }
      
    }
  }
}