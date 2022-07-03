using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Infrastructure.Config;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Models
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 获取客户Ip
        /// </summary>
        /// <param name="controllerBase"></param>
        /// <returns></returns>
        public static string GetClientIP(this ControllerBase controllerBase)
        {
            var ip = controllerBase.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(ip))
            {
                return controllerBase.HttpContext.Connection.RemoteIpAddress.ToString().Replace("::1", "127.0.0.1");
            }
            return ip;
        }

        ///// <summary>
        ///// 添加到 Response Header
        ///// </summary>
        ///// <param name="controllerBase"></param>
        ///// <param name="key">键</param>
        ///// <param name="value">值</param>
        ///// <returns></returns>
        //public static void AddResponseHeader(this ControllerBase controllerBase, string key, string value)
        //{
        //    controllerBase.HttpContext.Response.Headers.Add(key, new StringValues(value));
        //}

        /// <summary>
        /// 获取 Claims Value
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetClaimsValue(this IEnumerable<Claim> claims, string type)
        {
            return claims.FirstOrDefault(t => t.Type == type)?.Value ?? "";
        }

        /// <summary>
        /// 获取 User Token
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        public static UserToken GetToken(this ClaimsPrincipal claimsPrincipal)
        {
            var token = TokenHelper.GetUserToken(claimsPrincipal);
            return token;
        }


        /// <summary>
        /// 序列化 User Token
        /// </summary>
        /// <param name="securityKey">密钥(对称密钥)</param>
        /// <returns></returns>
        public static string Serialization(this UserToken userToken, TokenManagement jwtAppSettings)
        {
            var token = TokenHelper.CreateToken(jwtAppSettings, userToken);
            return token;
        }      

        /// <summary>
        /// 获取设备串号
        /// </summary>
        /// <param name="controllerBase"></param>
        /// <returns></returns>
        public static string GetIMEI(this ControllerBase controllerBase)
        {
            var key = "IMEI";
            var header = controllerBase.HttpContext.Request.Headers[key];
            if (header.Count == 0)
            {
                header = controllerBase.HttpContext.Request.Headers["imei"];
                if (header.Count == 0) return string.Empty;
            }
            return header.ToString();
        }

        ///// <summary>
        ///// 获取版本号
        ///// </summary>
        ///// <param name="controllerBase"></param>
        ///// <returns></returns>
        //public static string GetVersion(this ControllerBase controllerBase)
        //{
        //    var key = "Version";
        //    var header = controllerBase.HttpContext.Request.Headers[key];
        //    if (header.Count == 0) return string.Empty;
        //    return header.ToString();
        //}

        ///// <summary>
        ///// 获取渠道
        ///// </summary>
        ///// <param name="controllerBase"></param>
        ///// <returns></returns>
        //public static string GetChannel(this ControllerBase controllerBase)
        //{
        //    var key = "Channel";
        //    var header = controllerBase.HttpContext.Request.Headers[key];
        //    if (header.Count == 0) return string.Empty;
        //    return header.ToString();
        //}

        ///// <summary>
        ///// 获取显示语言
        ///// </summary>
        ///// <param name="controllerBase"></param>
        ///// <returns></returns>
        //public static string GetLanguage(this ControllerBase controllerBase)
        //{
        //    var key = "Language";
        //    var header = controllerBase.HttpContext.Request.Headers[key];
        //    if (header.Count == 0) return string.Empty;
        //    return header.ToString();
        //}

        ///// <summary>
        ///// 获取包名
        ///// </summary>
        ///// <param name="controllerBase"></param>
        ///// <returns></returns>
        //public static string GetPackage(this ControllerBase controllerBase)
        //{

        //    var key = "Package";
        //    var header = controllerBase.HttpContext.Request.Headers[key];
        //    if (header.Count == 0) return string.Empty;
        //    return header.ToString();
        //}

    }

}
