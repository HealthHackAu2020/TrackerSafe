using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TrackerSafe.Shared;

namespace app.Extensions
{
  public static class HttpClientExtensions
  {
    public static async Task<T> GetApiData<T, P>(this HttpClient http, NavigationManager navManager, ILogger<P> logger, string url)
    {
      const int retryCount =  3;
      int currentRetry = 0;
      try
      {
        HttpResponseMessage remoteResults = null;
        var successful = false;
        while (!successful && currentRetry < retryCount)
        {
          currentRetry++;
          remoteResults = await http.GetAsync(url);
          if (remoteResults.StatusCode != HttpStatusCode.InternalServerError)
          {
            successful = true;
          }
        }
        Helpers.ApiResponse.HandleUnsuccessfulApiResponse(navManager, logger, remoteResults);
        string jsonString = await remoteResults.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<T>(jsonString);
        return result;
      }
      catch (Exception ex)
      {
        Helpers.ApiResponse.HandleUnsuccessfulApiResponse(navManager, logger, ex);
        throw;
      }
    }

    public static async Task<T> PostApiData<T, P>(this HttpClient http, NavigationManager navManager, ILogger<P> logger, Blazored.LocalStorage.ISyncLocalStorageService localStorage, string url, object requestPayload) where T : class
    {
      try
      {
        var sessionId = localStorage.GetSessionId();
        var jwt = localStorage.GetJwt();

        var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(url),
            Headers = { 
              { Constants.HttpHeaderSessionId, sessionId },
              { HttpRequestHeader.Authorization.ToString(), $"Bearer {jwt}" }
            },
            Content = new StringContent(JsonConvert.SerializeObject(requestPayload))
        };
        var remoteResults = await http.SendAsync(requestMessage);
        Helpers.ApiResponse.HandleUnsuccessfulApiResponse(navManager, logger, remoteResults);
        string resultString = await remoteResults.Content.ReadAsStringAsync();
        if (typeof(T) == typeof(string))
        {
          return resultString as T;
        }
        var result = JsonConvert.DeserializeObject<T>(resultString);
        return result;
      }
      catch (Exception ex)
      {
        Helpers.ApiResponse.HandleUnsuccessfulApiResponse(navManager, logger, ex);
        throw;
      }
    }
  }
}