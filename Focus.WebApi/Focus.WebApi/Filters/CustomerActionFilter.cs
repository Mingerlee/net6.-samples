using Infrastructure.CacheManager;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Focus.WebApi.Filters
{
    public class CustomerActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string route = context.HttpContext.Request.Path;
            string ip = Infrastructure.Utilities.HttpContextHelper.GetIp;
            string id = context.HttpContext.TraceIdentifier;
            string cacheKey = $"{ip.Replace('.', '_')}_{route.Replace('/', '_')}_{id}";
            CacheContext.Cache.AddObject(cacheKey, context.ActionArguments);//存入缓存
        }
    }
}
