using Focus.Repository.DBManager;
using Focus.WebApi.Attributes;
using Infrastructure.Config;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Profiling;

namespace Focus.WebApi.Filters
{
    public class SQLProfilerFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 获取请求进来的控制器与方法
            //var isDefined = AppSetting.GetConfigBoolean("SysSetting:SQLProfiler");
            //var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            //if (controllerActionDescriptor != null)
            //{
            //    isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
            //      .Any(a => a.GetType().Equals(typeof(SQLProfilerAttribute)));
            //}
            if (AppSetting.GetConfigBoolean("SysSetting:SQLProfiler"))
            {
                var profiler = MiniProfiler.StartNew("StartNew");
                using (profiler.Step("SqlProfiler"))
                {
                    await next();
                }
                WriteDbSqlLog(profiler);//记录执行SQL语句
            }
            else
                await next();
        }

        private void WriteDbSqlLog(MiniProfiler profiler)
        {
            string ActionSqlString = string.Empty;
            decimal execTime = 0;
            if (profiler.Root != null)
            {
                var p = profiler.Root;
                execTime =Math.Abs(p.DurationWithoutChildrenMilliseconds);
                if (p.HasChildren)
                {
                    p.Children.ForEach(x =>
                    {
                        if (x.CustomTimings?.Count > 0)
                        {
                            foreach (var ct in x.CustomTimings)
                            {
                                ct.Value?.ForEach(y =>
                                {
                                    ActionSqlString += $"{y.CommandString};";
                                });
                            }
                        }
                    });
                }
                if (!string.IsNullOrEmpty(ActionSqlString))
                {
                    //记录日志,写入DB SysSqlLog表
                    //Console.WriteLine($"执行时间：{execTime}(ms)  执行的SQL语句：{ActionSqlString}");
                    SysSqlLogManager.WriteSqlLog(ActionSqlString, execTime);
                }
            }
        }
    }
}
