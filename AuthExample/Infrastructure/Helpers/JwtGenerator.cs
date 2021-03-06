using AuthExample.Infrastructure.Constants;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthExample.Infrastructure.Helpers
{
    public static class JwtGenerator
    {

        /// <summary>
        /// Generates short-lived token for protected controller
        /// </summary>
        /// <returns></returns>
        public static (string Token, DateTime ExpirationDate) GenerateToken(string key, string userId, string iss, bool isAdmin)
        {
            var secBytes = Encoding.UTF8.GetBytes(key);
            var securityKey = new SymmetricSecurityKey(secBytes);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var nbf = DateTime.UtcNow;
            var expirationTime = nbf.AddSeconds(300);
            var claims = new List<Claim>();
            if(isAdmin)
                claims.Add(new Claim(ClaimTypes.Role, AuthRoles.Admin));
            else
                claims.Add(new Claim(ClaimTypes.Role, AuthRoles.User));
            claims.Add(new Claim(ClaimTypes.Name, userId));

            JwtSecurityToken token = new JwtSecurityToken(
                iss,
                "",
                claims,
                nbf,
                expirationTime,
                signingCredentials
            );

            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(token);
            return (tokenString, expirationTime);
        }
    }
}
