using System;

namespace app.Extensions
{
  public static class NavigationManagerExtensions
  {
    public static void NavigateToPage(this Microsoft.AspNetCore.Components.NavigationManager navManager, string pageUrl, bool forceLoad = false)
    {
      var finalUrl = navManager.BaseUri.Contains("healthhackau2020.github.io", StringComparison.CurrentCultureIgnoreCase) ? $"/TrackerSafe{pageUrl}" : pageUrl;
      navManager.NavigateTo(finalUrl, forceLoad);
    }
  }
}