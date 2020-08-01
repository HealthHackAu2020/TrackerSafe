namespace TrackerSafe.App
{
  public static class AppConstants
  {
    public const string PageUrlHome = "/";
    public const string PageUrlHomeLoggedIn = PageUrlMyProfile; //the default page to go to after logging in, can change..
    public const string PageUrlSettings = "/settings";
    public const string PageUrlSignUp = "/signup";
    public const string PageUrlLogin = "/login";
    public const string PageUrlLogout = "/logout";
    public const string PageUrlGetLocation = "/get-location";
    public const string PageUrlGetLocationTest = "/get-location-test";
    public const string PageUrlPushNotifications = "/push-notifications";
    public const string PageUrlPushNotificationsTest = "/push-notifications-test";
    public const string PageUrlInstalledApp = "/installed-app";
    public const string PageUrlMyProfile = "/my-profile";
    public const string PageUrlCheckIn = "/check-in";
    public const string PageUrlRewards = "/rewards";
    public const string PageUrlLearnMore = "/learn-more";
    public const string PageUrlTeams = "/teams";
    public const string PageUrlReferralCode = "/referral-code";
    public const string PageUrlViewNotifications = "/view-notifications";
    public const string InvalidSession = "/invalid-session";
    public const string UnexpectedError = "/unexptected-error";
  }
}