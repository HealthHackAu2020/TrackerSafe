// https://github.com/BenMorris/FunctionsCustomSercuity/blob/master/dependency-injection/AccessTokens/AccessTokenProvider.cs
namespace TrackerSafe.Backend.Security
{
  using System;
  using System.IdentityModel.Tokens.Jwt;
  using System.Security.Claims;
  using System.Text;
  using Microsoft.AspNetCore.Http;
  using Microsoft.IdentityModel.Tokens;

  /// <summary>
  /// Validates a incoming request and extracts any <see cref="ClaimsPrincipal"/> contained within the bearer token.
  /// </summary>
  public class AccessTokenProvider : IAccessTokenProvider
  {
    private const string AUTH_HEADER_NAME = "Authorization";
    private const string BEARER_PREFIX = "Bearer ";
    private readonly string _issuerToken;
    private readonly string _audience;
    private readonly string _issuer;

    public AccessTokenProvider(string issuerToken, string audience, string issuer)
    {
      _issuerToken = issuerToken;
      _audience = audience;
      _issuer = issuer;
    }

    public string GenerateToken(string userId, string userName)
    {
      var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_issuerToken));

      var tokenHandler = new JwtSecurityTokenHandler();
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
        {
          new Claim(ClaimTypes.NameIdentifier, userName),
          new Claim(ClaimTypes.Sid, userId)
        }),
        Expires = DateTime.UtcNow.AddDays(14),
        Issuer = _issuer,
        Audience = _audience,
        SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

    public AccessTokenResult ValidateToken(HttpRequest request)
    {
      try
      {
        if (request != null &&
            request.Headers.ContainsKey(AUTH_HEADER_NAME) &&
            request.Headers[AUTH_HEADER_NAME].ToString().StartsWith(BEARER_PREFIX))
        {
          var token = request.Headers[AUTH_HEADER_NAME].ToString().Substring(BEARER_PREFIX.Length);

          if (string.IsNullOrWhiteSpace(token?.Trim()))
          {
            return AccessTokenResult.NoToken();
          }

          var tokenParams = new TokenValidationParameters()
          {
            RequireSignedTokens = true,
            ValidAudience = _audience,
            ValidateAudience = true,
            ValidIssuer = _issuer,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_issuerToken))
          };

          var handler = new JwtSecurityTokenHandler();
          var result = handler.ValidateToken(token, tokenParams, out var securityToken);
          return AccessTokenResult.Success(result);
        }
        else
        {
          return AccessTokenResult.NoToken();
        }
      }
      catch (SecurityTokenExpiredException)
      {
        return AccessTokenResult.Expired();
      }
      catch (Exception ex)
      {
        return AccessTokenResult.Error(ex);
      }
    }
  }
}