using Infrastructure.CacheManager;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Focus.WebApi.Filters
{
    public class CustomerResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            string route = context.HttpContext.Request.Path;
            string ip = Infrastructure.Utilities.HttpContext.GetIp;
            string id = context.HttpContext.TraceIdentifier;

            string cacheKey = $"{ip.Replace('.', '_')}_{route.Replace('/', '_')}_{id}";
            CacheContext.Cache.Remove(cacheKey);
            //throw new NotImplementedException();
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
