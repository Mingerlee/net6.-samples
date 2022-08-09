using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Focus.Repository.Models;

namespace Focus.WebApi.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    public static class AutoMapperSetup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {

            if (services == null) throw new ArgumentNullException(nameof(services));

            //services.AddAutoMapper(typeof(DomainToViewModelMappingProfile), typeof(ViewModelToDomainMappingProfile));
            services.AddAutoMapper(typeof(AutoMapperProfile));
            
        }
    }
}
