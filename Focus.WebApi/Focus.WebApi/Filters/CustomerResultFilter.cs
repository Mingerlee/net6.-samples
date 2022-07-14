using Infrastructure.CacheManager;
using Microsoft.AspNetCore.Mvc.Filters;
using Infrastructure.Utilities;

namespace Focus.WebApi.Filters
{
    public class CustomerResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            string route = context.HttpContext.Request.Path;
            string ip = HttpContextHelper.GetIp;
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
