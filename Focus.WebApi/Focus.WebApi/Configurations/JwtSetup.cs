using Infrastructure.Config;
using Infrastructure.DEncrypt;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Focus.WebApi.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    public static class JwtSetup
    {
        /// <summary>
        /// jwt配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddJwtAuthSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            // JWT Setup
            var appSettingsSection = configuration.GetSection("TokenManagement");
            services.Configure<TokenManagement>(appSettingsSection);
            var tokenManagement = appSettingsSection.Get<TokenManagement>();
            var key = Encoding.ASCII.GetBytes(AESEncrypt.GetMd5(tokenManagement.Secret));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,//是否验证Issuer
                    ValidateAudience = false,//是否验证Audience
                    ValidateLifetime = false,//是否验证失效时间
                    ValidateIssuerSigningKey = false,//是否验证SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(key),//拿到SecurityKey
                    ValidAudience = tokenManagement.Audience,
                    ValidIssuer = tokenManagement.Issuer

                };
                options.Events = new JwtBearerEvents()
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.Clear();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 401;
                        context.Response.WriteAsync(new { message = "授权未通过", status = false, code = 401 }.Serialize());
                        return Task.CompletedTask;
                    }
                };
            });

        }
    }

}