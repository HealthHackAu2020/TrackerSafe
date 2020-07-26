namespace TrackerSafe.Shared
{
  public class SignUpUserRequest
  {
    public SignUpUserRequest()
    {
    }
    public SignUpUserRequest(string userName, string password, string confirmPassword, string referralCode)
    {
      UserName = userName;
      Password = password;
      ConfirmPassword = confirmPassword;
      ReferralCode = referralCode;
    }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }

    public string ReferralCode { get; set; }
  }
}