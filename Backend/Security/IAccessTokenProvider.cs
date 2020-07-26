// https://github.com/BenMorris/FunctionsCustomSercuity/blob/master/dependency-injection/AccessTokens/IAccessTokenProvider.cs
namespace TrackerSafe.Backend.Security
{
  using Microsoft.AspNetCore.Http;

  /// <summary>
  /// Validates access tokes that have been submitted as part of a request.
  /// </summary>
  public interface IAccessTokenProvider
  {
    /// <summary>
    /// Generate an access token, returning the token as a result.
    /// </summary>
    /// <param name="userId">The userid (Sid) of the user.</param>
    /// <param name="userName">The username (NameIdentifier) of the user.</param>
    /// <returns>A result that contains the security principal.</returns>
    string GenerateToken(string userId, string userName);

    /// <summary>
    /// Validate the access token, returning the security principal in a result.
    /// </summary>
    /// <param name="request">The HTTP request containing the access token.</param>
    /// <returns>A result that contains the security principal.</returns>
    AccessTokenResult ValidateToken(HttpRequest request);
  }
}