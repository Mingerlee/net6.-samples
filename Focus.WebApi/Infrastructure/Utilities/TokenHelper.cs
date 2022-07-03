using Infrastructure.Config;
using Infrastructure.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Infrastructure.Enums;
using System.Security.Cryptography;
using Infrastructure.DEncrypt;

namespace Infrastructure.Utilities
{
    /// <summary>
    /// Token 生成帮助类
    /// </summary>
    public class TokenHelper
    {
        /// <summary>
        /// 获取用户 Token
        /// </summary>
        /// <returns></returns>
        public static UserToken GetUserToken(ClaimsPrincipal cp)
        {
            var claims = cp.Claims;

            var uid = claims.GetClaimsValue(ClaimTypes.Name);
            var email = claims.GetClaimsValue(ClaimTypes.NameIdentifier);

            var userToken = new UserToken
            {
                UID = string.IsNullOrEmpty(uid) ? string.Empty : uid,

                Email = email,
                UserCode = claims.GetClaimsValue("usercode"),
                Name = claims.GetClaimsValue("name"),
                Mobile = claims.GetClaimsValue("mobile"),
                MobileArea = claims.GetClaimsValue("mobilearea"),
                Version = claims.GetClaimsValue("version"),
                IP = claims.GetClaimsValue("ip"),
                Platform = claims.GetClaimsValue("platform").ToEnum(EnumPlatformType.PC),
                Channel = claims.GetClaimsValue("channel"),
                IMEI = claims.GetClaimsValue("imei"),
                Account=claims.GetClaimsValue("account").ToInt32(0),
            };
            return userToken;
        }
     
        /// <summary>
        /// 创建用户 Token
        /// </summary>
        /// <param name="securityKey">密钥(对称密钥)</param>
        /// <param name="userToken">Token</param>
        /// <returns></returns>
        public static string CreateToken(TokenManagement settings, UserToken userToken)
        {
            // 将用户的名字推入一个声明中，这样我们就可以稍后确定用户的身份。
            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, userToken.UID.ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUniversalTime().ToString(),ClaimValueTypes.Integer64),
                    new Claim(JwtRegisteredClaimNames.UniqueName, userToken.UID.ToString()),
                    new Claim("usercode",userToken.UserCode?? ""),
                    new Claim("name",userToken.Name?? ""),
                    new Claim("imei", userToken.IMEI ?? ""),
                    new Claim("version", userToken.Version),
                    new Claim("email", userToken.Email ?? ""),
                    new Claim("mobile", userToken.Mobile?? ""),
                    new Claim("mobilearea", userToken.MobileArea),
                    new Claim("account",userToken.Account.ToString()),
                    new Claim("platform", ((int)userToken.Platform).ToString()),
                    new Claim("channel", userToken.Channel??""),
                    new Claim("ip", userToken.IP??""),
                    new Claim(ClaimTypes.Role, "Administrator")
                };

            // 创建一个密钥(对称密钥)
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AESEncrypt.GetMd5(settings.Secret)));          
            // 指定创建签名方式
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //.NET Core’s JwtSecurityToken class takes on the heavy lifting and actually creates the token.

            //* Claims (Payload)
            //   Claims 部分包含了一些跟这个 token 有关的重要信息。 JWT 标准规定了一些字段，下面节选一些字段:

            //   iss: The issuer of the token，token 是给谁的
            //   sub: The subject of the token，token 主题
            //   exp: Expiration Time。 token 过期时间，Unix 时间戳格式
            //   iat: Issued At。 token 创建时间， Unix 时间戳格式
            //   jti: JWT ID。针对当前 token 的唯一标识
            //   除了规定的字段外，可以包含其他任何 JSON 兼容的字段。
            var token = new JwtSecurityToken(
                issuer: settings.Issuer,               // 发行人
                audience: settings.Audience,             // 受众/听众
                claims: claims,
                signingCredentials: creds,               // 指定使用哪个安全密钥和什么算法来创建签名
                expires: DateTime.Now.AddMinutes(settings.AccessExpiration)
                );
            return $"Bearer {new JwtSecurityTokenHandler().WriteToken(token)}";
        }
       

    }
}
