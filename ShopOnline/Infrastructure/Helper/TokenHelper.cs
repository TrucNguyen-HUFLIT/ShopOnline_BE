using Microsoft.IdentityModel.Tokens;
using ShopOnline.Appsetting;
using ShopOnline.Infrastructure.CurrentUsers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace PostalCode.Infrastructure.Helper
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
            var userId = int.Parse(claims.Where(x => x.Type == "certserialnumber").Select(x => x.Value).FirstOrDefault());

            return new CurrentUser
            {
                TypeAcc = role,
                UserId = userId,
            };
        }
    }
}
