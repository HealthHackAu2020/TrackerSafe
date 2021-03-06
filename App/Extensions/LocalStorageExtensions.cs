using System;
using TrackerSafe.Shared;
namespace app.Extensions
{
  public static class ILocalStorageServiceExtensions 
  {
    public static void SetJwt(this Blazored.LocalStorage.ISyncLocalStorageService localStorage, string value)
    {
      localStorage.SetItem("jwt", value);
    }
    public static string GetJwt(this Blazored.LocalStorage.ISyncLocalStorageService localStorage)
    {
      return localStorage.GetItem<string>("jwt");
    }

    public static string GetDeviceId(this Blazored.LocalStorage.ISyncLocalStorageService localStorage)
    {
      var res = localStorage.GetItem<string>("did");
      if (res == null)
      {
        //We only ever set this once, the first time they load up (or if they cleared local storage)
        res = Guid.NewGuid().ToString();
        localStorage.SetItem("did", res);
      }
      return res;
    }

    public static void SetSessionId(this Blazored.LocalStorage.ISyncLocalStorageService localStorage, string value)
    {
      localStorage.SetItem("sid", value);
    }
    public static string GetSessionId(this Blazored.LocalStorage.ISyncLocalStorageService localStorage)
    {
      return localStorage.GetItem<string>("sid");
    }

    public static void SetPushNotificationId(this Blazored.LocalStorage.ISyncLocalStorageService localStorage, Guid? value)
    {
      localStorage.SetItem("pid", value);
    }
    public static Guid? GetPushNotificationId(this Blazored.LocalStorage.ISyncLocalStorageService localStorage)
    {
      return localStorage.GetItem<Guid?>("pid");
    }

    public static void SetAppState(this Blazored.LocalStorage.ISyncLocalStorageService localStorage, AppState value)
    {
      localStorage.SetItem("aps", value);
    }
    public static AppState GetAppState(this Blazored.LocalStorage.ISyncLocalStorageService localStorage)
    {
      var appState = localStorage.GetItem<AppState>("aps");
      if (appState == null)
      {
        //We always need one, if there isn't one, start with default
        appState = new AppState();
        localStorage.SetItem("aps", appState);
      }
      return appState;
    }

    public static void SetUserName(this Blazored.LocalStorage.ISyncLocalStorageService localStorage, string value)
    {
      localStorage.SetItem("usr", value);
    }
    public static string GetUserName(this Blazored.LocalStorage.ISyncLocalStorageService localStorage)
    {
      return localStorage.GetItem<string>("usr");
    }

    public static void SetReferralCode(this Blazored.LocalStorage.ISyncLocalStorageService localStorage, string value)
    {
      localStorage.SetItem("ref", value);
    }
    public static string GetReferralCode(this Blazored.LocalStorage.ISyncLocalStorageService localStorage)
    {
      return localStorage.GetItem<string>("ref");
    }

    public static void SetSuccessMessage(this Blazored.LocalStorage.ISyncLocalStorageService localStorage, string value)
    {
      localStorage.SetItem("suc", value);
    }
    public static string GetSetSuccessMessage(this Blazored.LocalStorage.ISyncLocalStorageService localStorage)
    {
      return localStorage.GetItem<string>("suc");
    }

    public static void SetNumberOfAlerts(this Blazored.LocalStorage.ISyncLocalStorageService localStorage, int value)
    {
      localStorage.SetItem("nal", value);
    }
    public static int GetNumberOfAlerts(this Blazored.LocalStorage.ISyncLocalStorageService localStorage)
    {
      return localStorage.GetItem<int>("nal");
    }
  }
}