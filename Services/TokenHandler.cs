using System;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace backend.Services
{
  public static class TokenHandler
  {
    public static string CreateToken(string secret, string userId)
    {
      var key = Encoding.ASCII.GetBytes(secret);

      var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier, userId)
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
  }
}