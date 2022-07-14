using Autofac;
using Autofac.Extras.DynamicProxy;
using Infrastructur.AutofacManager;
using Infrastructure.CacheManager;
using Infrastructure.CacheManager.IService;
using Infrastructure.CacheManager.Service;
using Infrastructure.Config;
using Infrastructure.UserManager;
using Infrastructure.ValidationManager;
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
        /// <summary>
        /// 注册autofac
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterModule(this ContainerBuilder builder)
        {
            Type baseType = typeof(IDependency);
            //启用缓存
            if (AppSetting.GetConfigBoolean("RedisConfig:UseRedis"))
            {
                builder.RegisterType<RedisCacheService>().As<ICacheService>().SingleInstance();
            }
            else builder.RegisterType<MemoryCacheService>().As<ICacheService>().SingleInstance();

            builder.RegisterAssemblyTypes(GetAssemblyList())
             .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
             .AsSelf().AsImplementedInterfaces()
             .InstancePerLifetimeScope();

            builder.RegisterType<UserContext>().InstancePerLifetimeScope();
            //注册验证实体参数服务
            builder.RegisterType<ValidatorService>().As<IValidatorService>().SingleInstance();
            new ValidatorContainer();//初始化字典
        }
        /// <summary>
        /// 注册autofac代理
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterDynamicProxys(this ContainerBuilder builder)
        {
            Type baseType = typeof(IAutofacDynamicProxy);         

            builder.RegisterAssemblyTypes(GetAssemblyList())
                      .Where(type => baseType.IsAssignableFrom(type))
                      .AsImplementedInterfaces()
                      .InstancePerDependency()
                      .PropertiesAutowired()
                      .EnableInterfaceInterceptors();//引用Autofac.Extras.DynamicProxy;
                                                     //.InterceptedBy();//允许将拦截器服务的列表分配给注册。
        }
        /// <summary>
        /// 获取所有程序集下的类
        /// </summary>
        /// <returns></returns>
        private static Assembly [] GetAssemblyList()
        {
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

            return assemblyList.ToArray();
        }
    }
}
