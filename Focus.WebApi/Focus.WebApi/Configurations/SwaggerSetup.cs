using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Focus.WebApi.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    public static class SwaggerSetup
    {
        /// <summary>
        /// swagger配置
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("Focus", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Focus.WebApi Project",
                    Description = "Focus.WebApi API Swagger surface",

                });
                s.SwaggerDoc("ApiFox", new OpenApiInfo { 
                    Title = "ApiFox文档", 
                    Version = "v1", 
                    Description = "ApiFox文档"
                });
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, "Focus.WebApi.xml");
               // var xmlPath1 = Path.Combine(AppContext.BaseDirectory, "Samples.Service.APP.xml");
                //s.IgnoreObsoleteActions();
                s.DocInclusionPredicate((docName, description) => true);
                //s.IncludeXmlComments(xmlPath);
                //s.IncludeXmlComments(xmlPath1);
                var appXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var appXmlPath = Path.Combine(AppContext.BaseDirectory, appXmlFile);
                s.IncludeXmlComments(appXmlPath);
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerSetup(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/Focus/swagger.json", "Focus.WebApi Project");
                c.SwaggerEndpoint("/swagger/ApiFox/swagger.json", "ApiFox文档 v1");
            });
        }
    }
}
