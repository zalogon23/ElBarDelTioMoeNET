using System;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace backend.Services
{
  public static class TokenHandler
  {
    public static string CreateToken(string secret, string userId)
    {
      var key = Encoding.ASCII.GetBytes(secret);

      var claims = new[]{
                new Claim("userId", userId)
            };
      var claimsIdentity = new ClaimsIdentity(claims);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = claimsIdentity,
        Expires = DateTime.UtcNow.AddMinutes(2),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var tokenHandler = new JwtSecurityTokenHandler();
      var createdToken = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(createdToken);
    }
    public static string CreateRandomToken(string secret)
    {
      var randomNumber = new byte[32];
      string randomString;
      using (var rng = RandomNumberGenerator.Create())
      {
        rng.GetBytes(randomNumber);
        randomString = Convert.ToBase64String(randomNumber);
      }
      string randomToken = CreateToken(secret: secret, userId: randomString);
      return randomToken;
    }
  }
}