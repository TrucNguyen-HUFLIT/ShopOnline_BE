using Microsoft.IdentityModel.Tokens;
using ShopOnline.Core.Appsetting;
using ShopOnline.Core.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Core.Helper
{
    public static class TokenHelper
    {
        public static string GenerateToken(List<Claim> claims)
        {
            var handler = new JwtSecurityTokenHandler();
            var utcTimeNow = DateTime.Now;

            var subject = new ClaimsIdentity();

            subject.AddClaims(claims);

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfigs.BearerToken.SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = subject,
                SigningCredentials = signinCredentials,
                Expires = utcTimeNow.AddHours(AppConfigs.BearerToken.ExpireTime),
                IssuedAt = utcTimeNow,
                NotBefore = utcTimeNow,
                Issuer = AppConfigs.BearerToken.Issuer,
                Audience = AppConfigs.BearerToken.Audience
            });

            var token = handler.WriteToken(securityToken);

            return token;
        }

        public static CurrentUser GetUserInfo(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var tokenRead = handler.ReadToken(token) as JwtSecurityToken;

            var claims = tokenRead.Claims;

            var role = (TypeAcc)Enum.Parse(typeof(TypeAcc), claims.Where(x => x.Type == "role").Select(x => x.Value).FirstOrDefault(), true);

            return new CurrentUser
            {
                TypeAcc = TypeAcc.Staff,
                UserId = int.Parse(claims.Where(x => x.Type.Contains("id")).Select(x => x.Value).FirstOrDefault()),
                Email = claims.Where(x => x.Type.Contains("email")).Select(x => x.Value).FirstOrDefault(),
                FullName = claims.Where(x => x.Type.Contains("name")).Select(x => x.Value).FirstOrDefault(),
                Phone = claims.Where(x => x.Type.Contains("phone")).Select(x => x.Value).FirstOrDefault(),
                Address = claims.Where(x => x.Type.Contains("address")).Select(x => x.Value).FirstOrDefault(),
                Avatar = claims.Where(x => x.Type.Contains("avatar")).Select(x => x.Value).FirstOrDefault(),
            };
        }
    }
}
