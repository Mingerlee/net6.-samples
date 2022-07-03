using Autofac;
using Infrastructur.AutofacManager;
using Infrastructure.CacheManager;
using Infrastructure.CacheManager.IService;
using Infrastructure.CacheManager.Service;
using Infrastructure.Config;
using Infrastructure.UserManager;
using Microsoft.Extensions.DependencyModel;
using StackExchange.Profiling.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Infrastructure.AutofacManager
{
    public static class AutofacContainerModuleExtension
    {
        public static void RegisterModule(this ContainerBuilder builder)
        {
            Type baseType = typeof(IDependency);
            var compilationLibrary = DependencyContext.Default
                .CompileLibraries
                .Where(x => !x.Serviceable
                && x.Type == "project")
                .ToList();
            List<Assembly> assemblyList = new List<Assembly>();

            foreach (var _compilation in compilationLibrary)
            {
                try
                {
                    assemblyList.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(_compilation.Name)));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(_compilation.Name + ex.Message);
                }
            }
            //启用缓存
            if (AppSetting.GetConfigBoolean("RedisConfig:UseRedis"))
            {
                builder.RegisterType<RedisCacheService>().As<ICacheService>().SingleInstance();
            }
            else builder.RegisterType<MemoryCacheService>().As<ICacheService>().SingleInstance();

            builder.RegisterAssemblyTypes(assemblyList.ToArray())
             .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
             .AsSelf().AsImplementedInterfaces()
             .InstancePerLifetimeScope();

            builder.RegisterType<UserContext>().InstancePerLifetimeScope();        
        }
    }
}
