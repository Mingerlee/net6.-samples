using Infrastructure.CacheManager;
using Infrastructure.Config;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Infrastructure.Enums;
using Infrastructure.Utilities;

namespace Focus.WebApi.Filters
{
    public class CustomerExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public CustomerExceptionFilter(ILogger<CustomerExceptionFilter> logger)
        {
            _logger = logger;
        }
        public Task OnExceptionAsync(ExceptionContext context)
        {
            //开发环境不记录日志
            if (AppSetting.GetConfigBoolean("SysSetting:LogError"))
            {
                // 如果异常没有被处理则进行处理
                if (context.ExceptionHandled == false)
                {
                    _logger.LogError(BuilderMsg(context));
                    // 设置为true，表示异常已经被处理了
                    context.ExceptionHandled = true;
                    context.Result = new OkObjectResult(new ResultModel(0, ResponseCode.sys_exception, "网络请求超时,请稍后再试 !"));
                }
            }
            return Task.CompletedTask;
        }

        private string BuilderMsg(ExceptionContext context)
        {
            var request = context.HttpContext.Request;
            var pars = request.QueryString.ToString();

            if (request.Method.ToLower() == "post")
            {

                string route = context.HttpContext.Request.Path;
                string ip = HttpContextHelper.GetIp;
                string id = context.HttpContext.TraceIdentifier;

                string cacheKey = $"{ip.Replace('.', '_')}_{route.Replace('/', '_')}_{id}";
                Dictionary<string,object?> pairs= CacheContext.Cache.Get<Dictionary<string, object?>>(cacheKey);//存入缓存
                pars = "{";
                foreach (string key in pairs.Keys)
                {
                    
                    var value = pairs[key];
                    string par = $"{key}:"+ JsonConvert.SerializeObject(value);
                    pars += par+";";
                }
                pars += "}";
                CacheContext.Cache.Remove(cacheKey);//使用完成之后，移除缓存
            }
            var fromData = @"Path:{0},输入参数:{1},IP:{2}";

            fromData = string.Format(@fromData, request.Path.ToString(), pars, request.Host);

            var log = $"{fromData}-->{context.Exception.GetType()}：{context.Exception.Message}——异常堆栈信息：{context.Exception.StackTrace}";

            return log;
        }
    }
}
