using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

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
          navManager.NavigateTo("invalid-session", false);
        }
        else if (ex.Message.Contains("500"))
        {
          navManager.NavigateTo("unexpected-error", false);
        }
      }
    }

    public static void HandleUnsuccessfulApiResponse<T>(NavigationManager navManager, ILogger<T> logger, HttpResponseMessage msg)
    {
      if ((int)msg.StatusCode >= 300 && (int)msg.StatusCode < 500)
      {
        logger.LogError($"Error from API Call - status {msg.StatusCode}");
        navManager.NavigateTo("invalid-session", false);
      }
      else if ((int)msg.StatusCode >= 500)
      {
        logger.LogError($"Error from API Call - status {msg.StatusCode}");
        navManager.NavigateTo("unexpected-error", false);
      }
      
    }
  }
}