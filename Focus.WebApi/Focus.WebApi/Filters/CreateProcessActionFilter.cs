using Microsoft.AspNetCore.Mvc.Filters;

namespace Focus.WebApi.Filters
{
    public class CreateProcessActionFilter : IAsyncActionFilter
    {
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //throw new NotImplementedException();
            var results= next.Invoke();//执行action方法
            //foreach (var result in results)
            //{
            //    context.Result = result;
            //}
            //Console.WriteLine($"OnActionExecutionAsync:{DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
