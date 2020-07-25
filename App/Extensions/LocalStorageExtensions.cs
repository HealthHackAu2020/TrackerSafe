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

    public static void SetSessionId(this Blazored.LocalStorage.ISyncLocalStorageService localStorage, string value)
    {
      localStorage.SetItem("sid", value);
    }
    public static string GetSessionId(this Blazored.LocalStorage.ISyncLocalStorageService localStorage)
    {
      return localStorage.GetItem<string>("sid");
    }
  }
}