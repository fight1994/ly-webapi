using LY.Model.Share;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LY.Common.Helper
{
    public class JWTHelper
    {
        public static string CreateToken(JwtConfigModel jwtConfig, JwtUserInfo userInfo)
        {
            if (jwtConfig == null || userInfo == null) return string.Empty;

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Iss,jwtConfig.Issuer??""),
                new Claim("UserId",userInfo.UserId??""),
                new Claim("UserName",userInfo.UserName??""),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtConfig.Issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtConfig.Expires),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static JwtUserInfo TokenToModel(string token)
        {
            JwtUserInfo result = null;
            try
            {
                if (string.IsNullOrEmpty(token)) return null;

                var tokenEntity = new JwtSecurityTokenHandler().ReadJwtToken(token);
                if (tokenEntity != null)
                {
                    var userId = tokenEntity.Claims.Where(o => o.Type == "UserId")?.FirstOrDefault()?.Value;
                    var userName = tokenEntity.Claims.Where(o => o.Type == "UserName")?.FirstOrDefault()?.Value;
                    result = new JwtUserInfo()
                    {
                        UserId = userId,
                        UserName = userName,
                    };
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
    }
}
