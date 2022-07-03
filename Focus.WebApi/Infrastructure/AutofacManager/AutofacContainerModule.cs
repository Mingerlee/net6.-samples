using Autofac;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.AutofacManager
{
    public static class AutofacContainerModule
    {
       public static IContainer container { get; set; }

        public static TService GetService<TService>() where TService : class
        {
            //return ServiceProviderAccessor.ServiceProvider.GetService(typeof(TService)) as TService;

            return typeof(TService).GetService() as TService;
        }


        public static object GetService(this Type serviceType)
        {
            if (Utilities.HttpContext.Current == null) return ServiceProviderAccessor.ServiceProvider.GetService(serviceType);

            return Utilities.HttpContext.Current.RequestServices.GetService(serviceType);
        }

        public static TService GetContainerService<TService>() where TService : class
        {
            return container.Resolve<TService>();
        }
        
    }
    
}
